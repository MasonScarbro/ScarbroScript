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

        public int Arity => 0;

        public override string ToString()
        {
            return name;
        }
    }
}
