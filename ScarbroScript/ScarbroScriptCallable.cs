using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    // This is the definition of a callable it has an Arity (args) and A Call which passes in its args and tghe Interpreter if need be
    public interface ScarbroScriptCallable
    {
        int Arity { get; }
        Object Call(Interpreter interpreter, List<Object> arguments);
    }

}
