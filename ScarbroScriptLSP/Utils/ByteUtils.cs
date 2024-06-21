using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScarbroScriptLSP.Utils
{
    public class ByteUtils
    {
        public static bool Cut(byte[] msg, byte[] delimiter, out byte[] header, out byte[] content)
        {
            header = null;
            content = null;

            int index = IndexOf(msg, delimiter);
            if (index >= 0)
            {
                header = new byte[index];
                content = new byte[msg.Length - index - delimiter.Length];

                Array.Copy(msg, 0, header, 0, index);
                Array.Copy(msg, index + delimiter.Length, content, 0, msg.Length - index - delimiter.Length);

                return true;
            }

            return false;
        }

        private static int IndexOf(byte[] source, byte[] search)
        {
            for (int i = 0; i <= source.Length - search.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < search.Length; j++)
                {
                    if (source[i + j] != search[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i;
            }
            return -1;
        }
    }

}
