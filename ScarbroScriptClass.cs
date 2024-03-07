using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class ScarbroScriptClass : ScarbroScriptCallable
    {
        public readonly string name;
        public readonly Dictionary<string, ScarbroScriptFunction> methods;
        public ScarbroScriptClass(string name, Dictionary<string, ScarbroScriptFunction> methods)
        {
            this.name = name;
            this.methods = methods;
        }

        public Object Call(Interpreter interpreter, List<object> arguments)
        {
            ScarbroScriptInstance instance = new ScarbroScriptInstance(this);
            ScarbroScriptFunction initializer = FindMethod("init");
            /// <summary>
            /// When a class is called, after the ScarbroScriptInstance is created,
            /// we look for an “init” method. 
            /// If we find one, we immediately bind and invoke it
            /// just like a normal method call. The argument list is forwarded along.
            /// </summary>
            if (initializer != null)
            {
                initializer.Bind(instance).Call(interpreter, arguments);
            }
            return instance;
        }

        public ScarbroScriptFunction FindMethod(string name)
        {
            if (methods.TryGetValue(name, out ScarbroScriptFunction func))
            {
                return func;
            }
            return null;
        }

        public int Arity => GetArity(); //This is so dumb lol

        public int GetArity()
        {
            ScarbroScriptFunction initializer = FindMethod("init");
            if (initializer == null) return 0;
            return initializer.Arity;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
