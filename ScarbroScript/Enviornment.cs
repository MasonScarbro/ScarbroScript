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

        public Object GetAt(int distance, String name)
        {
            return Ancestor(distance).values[name];
        }

        public void AssignAt(int distance, Token name, Object value)
        {
            Ancestor(distance).values[name.lexeme] = value;
        }
        
        //walks a fixed number (the distance) in order to find the enviornment for the var
        public Enviornment Ancestor(int distance)
        {
            Enviornment enviornment = this;
            for (int i = 0; i < distance; i++)
            {
                enviornment = enviornment.enclosing;
            }

            return enviornment;
        }

        public void Define(String name, Object value)
        {
            //Adds the variable name and its associated value to the mappings of Vars!
            Console.WriteLine(values);
            values.Add(name, value);
        }

        public void Assign(Token name, Object value)
        {
            
            if (values.ContainsKey(name.lexeme))
            {
                //Assigns the variable the new value
                values.Remove(name.lexeme);
                values.Add(name.lexeme, value);
                //Also this ^^^ looks a little hackneed but no put function so whatta gonna dd 
                return;
            }

            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }

            throw new RuntimeError(name, "Undefined variable " + name.lexeme);
        }

        public void IndexAssignAt(int distance, Token name, object index, Object value)
        {

            var varVal = GetAt(distance, name.lexeme);
            if (varVal is List<object> arr)
            {


                arr[int.Parse(index.ToString())] = value;

            }
            else
            {
                throw new RuntimeError(name, "Undefined variable " + name.lexeme);
            }
            
        }

        public void IndexAssign(Token name, object index, Object value)
        {

            var varVal = Get(name);
            if (varVal is List<object> arr)
            {


                arr[int.Parse(index.ToString())] = value;

            }
            else
            {
                throw new RuntimeError(name, "Undefined variable " + name.lexeme);
            }
            
        }

    }
}
