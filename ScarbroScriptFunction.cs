using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class ScarbroScriptFunction : ScarbroScriptCallable
    {
        private readonly Stmt.Function declaration;
        private readonly Enviornment enclosureEnviornment;
        public ScarbroScriptFunction(Stmt.Function declaration, Enviornment enclosureEnviornment)
        {
            this.declaration = declaration;
            this.enclosureEnviornment = enclosureEnviornment;
        }

        public Object Call(Interpreter interpreter, List<Object> arguments)
        {
            Enviornment enviornment = new Enviornment(enclosureEnviornment);
            for (int i = 0; i < declaration.parameters.Count; i++)
            {
                enviornment.Define(declaration.parameters[i].lexeme, arguments[i]);
            }
            try
            {
                interpreter.ExecuteBlock(declaration.body, enviornment);
            }
            catch (Return returnObj)
            {
                return returnObj.value; //uses the return exception to get the value
            }
            //''else''
            return null;
            
        }


        public ScarbroScriptFunction Bind(ScarbroScriptInstance instance)
        {
            Enviornment env = new Enviornment(enclosureEnviornment);
            env.Define("this", instance);
            return new ScarbroScriptFunction(declaration, env);
        }

        public int Arity => declaration.parameters.Count;

        public override string ToString()
        {
            return "<fn " + declaration.name.lexeme + ">";
        }
    }
}
