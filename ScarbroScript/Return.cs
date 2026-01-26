using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScript
{
    public class Return : Exception
    {
        public readonly Object value;

        public Return(Object value) 
        {
            this.value = value;
        }
    }
}
