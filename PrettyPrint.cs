using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class PrettyPrintAST
    {
        public static void ASTPrint(List<Stmt> stmts)
        {
            Console.WriteLine("Statements:\n");
            foreach (Stmt stmt in stmts)
            {
                if (stmt is Stmt.Var stmtv)
                {
                    Console.WriteLine(
                        "{\n" +
                        "Initializer: " 
                        + stmtv.initializer + "\n"
                        + "Name: " 
                        + stmtv.name 
                        + "\n}");
                }
                if (stmt is Stmt.Expression stmte)
                {
                    Console.WriteLine("{\n" + "Expression: " + stmte.expression + "\n}");
                }
                if (stmt is Stmt.While stmtw)
                {
                    Console.WriteLine("{\n" + "Condition: " + stmtw.condition + "\n}");
                }
                
            }
            
            
        }

        
    }
}
