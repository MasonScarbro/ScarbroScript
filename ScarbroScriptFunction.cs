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
        private readonly bool isInitializer;
        public ScarbroScriptFunction(Stmt.Function declaration, Enviornment enclosureEnviornment, bool isInitializer)
        {
            this.declaration = declaration;
            this.enclosureEnviornment = enclosureEnviornment;
            this.isInitializer = isInitializer;
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
                if (isInitializer) return enclosureEnviornment.GetAt(0, "this", null);
                return returnObj.value; //uses the return exception to get the value
            }
            //''else''
            if (isInitializer) return enclosureEnviornment.GetAt(0, "this", null);
            return null;
            
        }


        public ScarbroScriptFunction Bind(ScarbroScriptInstance instance)
        {
            Enviornment env = new Enviornment(enclosureEnviornment);
            env.Define("this", instance);
            return new ScarbroScriptFunction(declaration, env, isInitializer);
        }

        public int Arity => declaration.parameters.Count;

        public override string ToString()
        {
            return "<fn " + declaration.name.lexeme + ">";
        }
    }
}
