using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Resolver : Expr.IVisitor<Object>, Stmt.IVisitor<object>
    {
        private readonly Interpreter interpreter;
        private readonly Stack<Dictionary<String, bool>> scopes = new Stack<Dictionary<String, bool>>();
        private FunctionType currentFunction = FunctionType.NONE;
       

        private enum FunctionType
        {
            NONE,
            FUNCTION,
            METHOD
        }

        public Resolver(Interpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public object VisitBlockStmt(Stmt.Block stmt)
        {
            BeginScope();
            Resolve(stmt.statements);
            EndScope();
            return null;
        }

        public object VisitClassStmt(Stmt.Class stmt)
        {
            Declare(stmt.name);
            Define(stmt.name);

            BeginScope();
            /// <summary>
            /// Whenever there is a 'this' expr inside a method it
            /// resolves to a local variable defined in an implicit scope
            /// outside the block of the method
            /// </summary>
            scopes.Peek()["this"] = true; //make this a variable in the current scope
            foreach (Stmt.Function method in stmt.methods)
            {
                FunctionType declaration = FunctionType.METHOD;
                ResolveFunction(method, declaration);
            }
            EndScope(); //discard the scope when done 
            return null;
        }

        public object VisitAccessExpr(Expr.Access expr)
        {
            Resolve(expr.obj); 
            return null;
        }

        public object VisitSetExpr(Expr.Set expr)
        {
            Resolve(expr.value);
            Resolve(expr.obj);
            return null;
        }


        public object VisitThisExpr(Expr.This expr)
        {
            ResolveLocal(expr, expr.keyword);
            return null;
        }

        public object VisitVarStmt(Stmt.Var stmt)
        {
            Declare(stmt.name);
            if (stmt.initializer != null)
            {
                Resolve(stmt.initializer);
            }
            //then
            Define(stmt.name);
            return null;
        }

        public object VisitAssignExpr(Expr.Assign expr)
        {
            Resolve(expr.value);
            ResolveLocal(expr, expr.name);
            return null;
        }

        public object VisitFunctionStmt(Stmt.Function stmt)
        {
            Declare(stmt.name); // Declare the name 
            Define(stmt.name); // Define the name, eagerly

            ResolveFunction(stmt, FunctionType.NONE); // "resolve" the body
            return null;

        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            if (!(scopes.Count == 0))
            {
                var isDeclared = scopes.Peek().TryGetValue(expr.name.lexeme, out var isDefined);
                if (isDeclared && !isDefined)
                {
                    ScarbroScript.Error(expr.name, "Cant read local var in its own initializer");
                }
            }
            //else
            ResolveLocal(expr, expr.name);
            return null;
        }

        //------- Other Visits -------//

        public object VisitExpressionStmt(Stmt.Expression stmt)
        {
            Resolve(stmt.expression);
            return null;
        }

        public object VisitIfStmt(Stmt.If stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.thenBranch);
            if (stmt.elseBranch != null) Resolve(stmt.elseBranch);
            return null;
        }

        public object VisitPrintStmt(Stmt.Print stmt)
        {
            Resolve(stmt.expression);
            return null;

        }

        public object VisitReturnStmt(Stmt.Return stmt)
        {
            if (currentFunction == FunctionType.NONE)
            {
                ScarbroScript.Error(stmt.keyword, "Can't return from top-level code.");
            }

            if (stmt.value != null)
            {
                Resolve(stmt.value);
            }
            return null;
        }

        public object VisitBreakStmt(Stmt.Break stmt)
        {
            return null;
        }

        public object VisitWhileStmt(Stmt.While stmt)
        {
            Resolve(stmt.condition);
            Resolve(stmt.body);
            return null;
        }

        public object VisitBinaryExpr(Expr.Binary expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);
            return null;
        }

        public object VisitCallExpr(Expr.Call expr)
        {
            Resolve(expr.callee);

            foreach (Expr argument in expr.arguments)
            {
                Resolve(argument);
            }

            return null;
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            Resolve(expr.expression);
            return null;
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return null;
        }

        public object VisitLogicalExpr(Expr.Logical expr)
        {
            Resolve(expr.left);
            Resolve(expr.right);
            return null;
        }


        public object VisitUnaryExpr(Expr.Unary expr)
        {
            Resolve(expr.right);
            return null;
        }

        //------- End Other Visits -------//

        public void Resolve(List<Stmt> statements)
        {
            foreach (Stmt statement in statements)
            {
                Resolve(statement);
            }
        }

        //Overloaded (Stmt), Basically the Execute function in interpreter
        private void Resolve(Stmt stmt)
        {
            stmt.Accept(this);
        }

        //Overloaded (Expr), Basically the Evaluate function in interpreter
        private void Resolve(Expr expr)
        {
            expr.Accept(this);
        }

        private void ResolveLocal(Expr expr, Token name)
        {
            for (int i = 0; i < scopes.Count; i++)
            {
                if (scopes.ElementAt(i).ContainsKey(name.lexeme))
                {
                    interpreter.Resolve(expr, i); // puts the expression and depth
                    return;
                }
            }
        }

        private void ResolveFunction(Stmt.Function function, FunctionType type)
        {
            FunctionType enclosingFunction = currentFunction;
            currentFunction = type;
            BeginScope();
            foreach (Token parameter in function.parameters)
            {
                Declare(parameter); // declare 
                Define(parameter); // and define params like vars (they basically are)

            }
            Resolve(function.body); // Execute/Resolve the body
            EndScope(); // exit scope
            currentFunction = enclosingFunction;
        }

        private void BeginScope()
        {
            // adds a new enviornment
            scopes.Push(new Dictionary<String, bool>());
        }


        private void EndScope()
        {
            // discards the latests enviornemnt 
            scopes.Pop();
        }

        private void Declare(Token name)
        {
            if (scopes.Count == 0) return;

            Dictionary<String, bool> scope = scopes.Peek();

            if (scope.ContainsKey(name.lexeme))
            {
                ScarbroScript.Error(name, $"The variable {name.lexeme} is already declared in the current scope.");
            }
            scope.Add(name.lexeme, false);
        }

        private void Define(Token name)
        {
            if (scopes.Count == 0) return;

            scopes.Peek()[name.lexeme] = true;

        }

        public object VisitArrayExpr(Expr.Array expr)
        {
            return null;
        }
    }
}
