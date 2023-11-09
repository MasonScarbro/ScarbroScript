using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    class Clock : ScarbroScriptCallable
    {
        //Arity is 0 due to no arguments
        public int Arity { get { return 0; } }

        /// <summary>
        ///  The Call method is where the actual implementation of the "clock"
        ///  function resides. It returns the current time in seconds
        ///  since a fixed point in time (usually the Unix epoch)
        ///  as a double value.
        /// </summary>
        /// <param name="interpreter"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public object Call(Interpreter interpreter, List<Object> arguments)
        {
            return DateTime.Now.Ticks / (double)TimeSpan.TicksPerSecond;
        }

        public override string ToString() => "<native fn>";
    }
}
