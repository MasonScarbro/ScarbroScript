using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class RuntimeError : Exception
    {
        public readonly Token token;

        public RuntimeError(Token token, String message) : base(message) //Base is he Super equivelent
        {
            this.token = token;
        }
    }
}
