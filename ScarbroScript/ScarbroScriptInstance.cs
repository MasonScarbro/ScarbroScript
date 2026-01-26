using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class ScarbroScriptInstance
    {
        private ScarbroScriptClass klass;
        private readonly Dictionary<string, object> fields = new Dictionary<string, object>();

        public ScarbroScriptInstance(ScarbroScriptClass klass)
        {
            this.klass = klass;
        }

        public object Get(Token name)
        {
            if (fields.ContainsKey(name.lexeme))
            {
                return fields[name.lexeme];
            }

            ScarbroScriptFunction method = klass.FindMethod(name.lexeme);
            if (method == null)
            {
                ScarbroScriptCallable mod = klass.FindMod(name.lexeme);
                
                if (mod != null) return mod;
            }
            if (method != null) return method.Bind(this);
            

            throw new RuntimeError(name, "Undefined Property '" + name.lexeme + "'");
        }

        public object Set(Token name, object value)
        {

            fields[name.lexeme] = value;
            return null;
        }

        public override string ToString()
        {
            return klass.name + " instance";
        }

        public string GetKlass()
        {
            return klass.name;
        }
    }
}
