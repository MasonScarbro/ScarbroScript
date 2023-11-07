using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Enviornment
    {
        public readonly Enviornment enclosing;
        private readonly Dictionary<String, Object> values = new Dictionary<String, Object>();
        //for the globals
        public Enviornment()
        {
            enclosing = null;
        }
        //for the nested (no globals)
        public Enviornment(Enviornment enclosing)
        {
            // creates a new local scope Enviornment
            this.enclosing = enclosing;
        }

        public Object Get(Token name)
        {
            if (values.ContainsKey(name.lexeme))
            {
                return values[name.lexeme];
            }

            if (enclosing != null) return enclosing.Get(name); // recursivley gets the enclosing valuess

            throw new RuntimeError(name, "Undefined variable: " + name.lexeme);
        }

        public void Define(String name, Object value)
        {
            //Adds the variable name and its associated value to the mappings of Vars!
            values.Add(name, value);
        }

        public void Assign(Token name, Object value)
        {
            
            if (values.ContainsKey(name.lexeme))
            {
                //Assigns the variable the new value
                values.Remove(name.lexeme);
                values.Add(name.lexeme, value);
                //Also this ^^^ looks a little hackeed but no put function so whatta gonna dd 
                return;
            }
            throw new RuntimeError(name, "Undefined variable " + name.lexeme);
        }

    }
}
