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
        private static readonly Dictionary<object, List<CompletionItem>> natives = new Dictionary<object, List<CompletionItem>>
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
            { "IO", new List<CompletionItem>
                {
                    new CompletionItem
                    {
                        Label = "readln",
                        Detail = "IO Function | 1 Parameter: any",
                        Documentation = "Starts an Input ReadFromConsole"

                    },

                }
            },
            { "File", new List<CompletionItem>
                {
                    new CompletionItem
                    {
                        Label = "write",
                        Detail = "File Function | 3 Parameter: string path, any text, string setting",
                        Documentation = "writes to a file (settings: {'w','a','ow','oa'})"

                    },
                    new CompletionItem
                    {
                        Label = "read",
                        Detail = "File Function | 1 Parameter: string path",
                        Documentation = "reads from a file"

                    },
                    new CompletionItem
                    {
                        Label = "readlns",
                        Detail = "File Function | 1 Parameter: string path",
                        Documentation = "reads all individual lines from a file"

                    },
                    new CompletionItem
                    {
                        Label = "mv",
                        Detail = "File Function | 2 Parameter: string pathFrom, string pathTo",
                        Documentation = "moves file to the other (unix based)"

                    },
                    new CompletionItem
                    {
                        Label = "rm",
                        Detail = "File Function | 1 Parameter: string path",
                        Documentation = "RemovesFile"

                    },

                }
            },
            { "Interope", new List<CompletionItem>
                {
                    new CompletionItem
                    {
                        Label = "executeMethod",
                        Detail = "Interope Function | 3 Parameter: string filePath, string namespace, string class, string method",
                        Documentation = "Executes a C# method"

                    },
                    new CompletionItem
                    {
                        Label = "generateExe",
                        Detail = "Interope Function | 3 Parameter: string filePath, string pathTo, string fileName",
                        Documentation = "Executes a C# file and generates a Exe"

                    },
                    new CompletionItem
                    {
                        Label = "executeFile",
                        Detail = "Interope Function | 1 Parameter: string path",
                        Documentation = "Compiles C# file"

                    },


                }
            },
            { typeof(List<>), new List<CompletionItem>
                {
                    new CompletionItem
                    {
                        Label = "append",
                        Detail = "Array Function | 1 Parameter: any value",
                        Documentation = "adds a value to the end of the array"

                    },
                    new CompletionItem
                    {
                        Label = "sizeOf",
                        Detail = "Array Function | No Parameters",
                        Documentation = "Returns the number of elements in the array"
                    },
                    new CompletionItem
                    {
                        Label = "remove",
                        Detail = "Array Function | 1 Parameter: any value",
                        Documentation = "Removes the first occurrence of the specified value from the array"
                    },
                    new CompletionItem
                    {
                        Label = "removeAt",
                        Detail = "Array Function | 1 Parameter: int index",
                        Documentation = "Removes the element at the specified index from the array"
                    },
                    new CompletionItem
                    {
                        Label = "contains",
                        Detail = "Array Function | 1 Parameter: any value",
                        Documentation = "Checks if the array contains the specified value"
                    },
                    new CompletionItem
                    {
                        Label = "reverse",
                        Detail = "Array Function | No Parameters",
                        Documentation = "Reverses the order of the elements in the array"
                    },
                    new CompletionItem
                    {
                        Label = "getAt",
                        Detail = "Array Function | 1 Parameter: int index",
                        Documentation = "Returns the element at the specified index in the array"
                    },
                    new CompletionItem
                    {
                        Label = "indexOf",
                        Detail = "Array Function | 1 Parameter: any value",
                        Documentation = "Returns the index of the first occurrence of the specified value in the array"
                    },
                    new CompletionItem
                    {
                        Label = "clear",
                        Detail = "Array Function | No Parameters",
                        Documentation = "Removes all elements from the array"
                    },
                    new CompletionItem
                    {
                        Label = "sort",
                        Detail = "Array Function | No Parameters",
                        Documentation = "Sorts the elements in the array"
                    },
                    new CompletionItem
                    {
                        Label = "concat",
                        Detail = "Array Function | 1 Parameter: List<T> other",
                        Documentation = "Concatenates the elements of the specified array to the end of this array"
                    }



                }
            },
            { typeof(Queue<>), new List<CompletionItem>
                {

                    new CompletionItem
                    {
                        Label = "enqueue",
                        Detail = "Queue Function | 1 Parameter: any value",
                        Documentation = "Adds a value to the end of the Queue"
                    },

                    new CompletionItem
                    {
                        Label = "printValues",
                        Detail = "Queue Function | 0 Parameters",
                        Documentation = "Prints all values in the Queue"
                    },

                    new CompletionItem
                    {
                        Label = "dequeue",
                        Detail = "Queue Function | 0 Parameters",
                        Documentation = "Removes and returns the first value from the Queue"
                    },

                    new CompletionItem
                    {
                        Label = "count",
                        Detail = "Queue Function | 0 Parameters",
                        Documentation = "Returns the number of elements in the Queue"
                    },

                    new CompletionItem
                    {
                        Label = "clear",
                        Detail = "Queue Function | 0 Parameters",
                        Documentation = "Removes all elements from the Queue"
                    },

                    new CompletionItem
                    {
                        Label = "peek",
                        Detail = "Queue Function | 0 Parameters",
                        Documentation = "Returns the first value in the Queue without removing it"
                    },
                }
            },
            { typeof(Stack<>), new List<CompletionItem>
                {

                    new CompletionItem
                    {
                        Label = "push",
                        Detail = "Stack Function | 1 Parameter: any value",
                        Documentation = "Adds a value to the top of the stack"
                    },

                    new CompletionItem
                    {
                        Label = "printValues",
                        Detail = "Stack Function | 0 Parameters",
                        Documentation = "Prints all values in the stack"
                    },

                    new CompletionItem
                    {
                        Label = "pop",
                        Detail = "Stack Function | 0 Parameters",
                        Documentation = "Removes and returns the value from the top of the stack"
                    },

                    new CompletionItem
                    {
                        Label = "count",
                        Detail = "Stack Function | 0 Parameters",
                        Documentation = "Returns the number of elements in the stack"
                    },

                    new CompletionItem
                    {
                        Label = "clear",
                        Detail = "Stack Function | 0 Parameters",
                        Documentation = "Removes all elements from the stack"
                    },

                    new CompletionItem
                    {
                        Label = "peek",
                        Detail = "Stack Function | 0 Parameters",
                        Documentation = "Returns the value at the top of the stack without removing it"
                    },
                }
            },
            { typeof(HashSet<>), new List<CompletionItem>
                {

                    new CompletionItem
                    {
                        Label = "add",
                        Detail = "Dictionary Function | 2 Parameters: key, value",
                        Documentation = "Adds a key-value pair to the dictionary"
                    },

                    new CompletionItem
                    {
                        Label = "remove",
                        Detail = "Dictionary Function | 1 Parameter: key",
                        Documentation = "Removes the key-value pair with the specified key from the dictionary"
                    },

                    new CompletionItem
                    {
                        Label = "contains",
                        Detail = "Dictionary Function | 1 Parameter: key",
                        Documentation = "Checks if the dictionary contains the specified key"
                    },

                    new CompletionItem
                    {
                        Label = "getVal",
                        Detail = "Dictionary Function | 1 Parameter: key",
                        Documentation = "Returns the value associated with the specified key"
                    },

                    new CompletionItem
                    {
                        Label = "getVals",
                        Detail = "Dictionary Function | 0 Parameters",
                        Documentation = "Returns a collection of all values in the dictionary"
                    },

                    new CompletionItem
                    {
                        Label = "getKeys",
                        Detail = "Dictionary Function | 0 Parameters",
                        Documentation = "Returns a collection of all keys in the dictionary"
                    },
                }
            },
            { typeof(string), new List<CompletionItem>
                {

                    new CompletionItem
                    {
                        Label = "substring",
                        Detail = "String Function | 2 Parameters: startIndex, length",
                        Documentation = "Returns a substring starting from the specified index with the specified length"
                    },
                    new CompletionItem
                    {
                        Label = "toLower",
                        Detail = "String Function | 0 Parameters",
                        Documentation = "Converts all characters in the string to lower case"
                    },
                    new CompletionItem
                    {
                        Label = "toUpper",
                        Detail = "String Function | 0 Parameters",
                        Documentation = "Converts all characters in the string to upper case"
                    },
                    new CompletionItem
                    {
                        Label = "indexOf",
                        Detail = "String Function | 1 Parameter: value",
                        Documentation = "Returns the index of the first occurrence of the specified value in the string"
                    },
                    new CompletionItem
                    {
                        Label = "trim",
                        Detail = "String Function | 0 Parameters",
                        Documentation = "Removes all leading and trailing white-space characters from the string"
                    },
                    new CompletionItem
                    {
                        Label = "replace",
                        Detail = "String Function | 2 Parameters: oldValue, newValue",
                        Documentation = "Replaces all occurrences of a specified value in the string with another value"
                    },
                    new CompletionItem
                    {
                        Label = "length",
                        Detail = "String Function | 0 Parameters",
                        Documentation = "Returns the length of the string"
                    },
                    new CompletionItem
                    {
                        Label = "startsWith",
                        Detail = "String Function | 1 Parameter: value",
                        Documentation = "Determines whether the beginning of the string matches the specified value"
                    },
                    new CompletionItem
                    {
                        Label = "endsWith",
                        Detail = "String Function | 1 Parameter: value",
                        Documentation = "Determines whether the end of the string matches the specified value"
                    },
                    new CompletionItem
                    {
                        Label = "split",
                        Detail = "String Function | 1 Parameter: delimiter",
                        Documentation = "Splits the string into an array of substrings based on the specified delimiter"
                    },
                    new CompletionItem
                    {
                        Label = "splitBy",
                        Detail = "String Function | 1 Parameter: delimiter",
                        Documentation = "Splits the string into an array of substrings based on the specified delimiter (alternative method)"
                    }
                }
            },
            { typeof(double), new List<CompletionItem>
                {

                    new CompletionItem
                    {
                        Label = "TEST",
                        Detail = "Double Function | 2 Parameters: startIndex, length",
                        Documentation = "Returns a substring starting from the specified index with the specified length"
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
        public static List<CompletionItem> TryGetNatives(object line)
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
