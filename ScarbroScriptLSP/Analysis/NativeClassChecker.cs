using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarbroScriptLSP.LSP;

namespace ScarbroScriptLSP.Analysis
{
    public class NativeClassChecker
    {
        private static readonly Dictionary<string, List<CompletionItem>> natives = new Dictionary<string, List<CompletionItem>>
        {
            { "Math", new List<CompletionItem>
                {
                    new CompletionItem
                    {
                        Label = "cos",
                        Detail = "Math Function | 1 Parameter: int x",
                        Documentation = "The cos(x) function from math"
                        
                    },
                    new CompletionItem
                    {
                        Label = "sin",
                        Detail = "Math Function | 1 Parameter: int x",
                        Documentation = "The sin(x) function from math"

                    },
                    new CompletionItem
                    {
                        Label = "tan",
                        Detail = "Math Function | 1 Parameter: int x",
                        Documentation = "The tan(x) function from math"

                    },
                    new CompletionItem
                    {
                        Label = "random",
                        Detail = "Math Function | 2 Parameter: int startRange, int endRange",
                        Documentation = "generates a random number from 2 ranges"

                    },
                    new CompletionItem
                    {
                        Label = "exp",
                        Detail = "Math Function | 2 Parameter: int base, int exp",
                        Documentation = "takes the exponent of the nums"

                    },
                    new CompletionItem
                    {
                        Label = "sqrt",
                        Detail = "Math Function | 1 Parameter: int root",
                        Documentation = "takes the sqrt of the number"

                    },
                    new CompletionItem
                    {
                        Label = "abs",
                        Detail = "Math Function | 1 Parameter: int num",
                        Documentation = "takes the absolute value of the num"

                    },
                    new CompletionItem
                    {
                        Label = "ceil",
                        Detail = "Math Function | 1 Parameter: int num",
                        Documentation = "takes the ceiling of the num"

                    },
                    new CompletionItem
                    {
                        Label = "floor",
                        Detail = "Math Function | 1 Parameter: int num",
                        Documentation = "takes the floored value of the num"

                    },
                    new CompletionItem
                    {
                        Label = "pow",
                        Detail = "Math Function | 2 Parameter: int base, int exp",
                        Documentation = "takes the base value and uhhh..."

                    },
                }
            },
            

        };

        /// <summary>
        /// This will return an empty list for now but it
        /// might throw an intentional error if it doesnt find
        /// anything....
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static List<CompletionItem> TryGetNatives(string line)
        {
            if (natives.ContainsKey(line))
            {
                return natives[line];
            }
            else
            {
                Program.logger.Log("Inside TryGetNatives and nothing was found :(");
                return new List<CompletionItem> { };
            }
        }

    }
}
