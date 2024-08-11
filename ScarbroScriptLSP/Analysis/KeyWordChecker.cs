using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarbroScriptLSP.LSP;

namespace ScarbroScriptLSP.Analysis
{
    public static class KeyWordChecker
    {
        
        private static readonly Dictionary<string, CompletionItem> keywords = new Dictionary<string, CompletionItem>
        {
            {"if",
                new CompletionItem
                {
                    Label = "if",
                    Detail = "if Keyword",
                    Documentation = "boolean statement checker"
                }
            },
            {"exists",
                new CompletionItem
                {
                    Label = "exists",
                    Detail = "exists Keyword",
                    Documentation = "checks if the prefixed item 'exists'"
                }
            },
            {"while",
                new CompletionItem
                {
                    Label = "while",
                    Detail = "while Keyword",
                    Documentation = "loop operator keyword"
                }
            },
            {"for",
                new CompletionItem
                {
                    Label = "for",
                    Detail = "for Keyword",
                    Documentation = "loop with counter keyword"
                }
            },
            {"foreach",
                new CompletionItem
                {
                    Label = "foreach",
                    Detail = "foreach Keyword",
                    Documentation = "loop with variable"
                }
            },
            {"else",
                new CompletionItem
                {
                    Label = "else",
                    Detail = "else Keyword",
                    Documentation = "alternative branch for 'if' statement"
                }
            },
            {"switch",
                new CompletionItem
                {
                    Label = "switch",
                    Detail = "switch Keyword",
                    Documentation = "multi-way branch statement"
                }
            },
            {"case",
                new CompletionItem
                {
                    Label = "case",
                    Detail = "case Keyword",
                    Documentation = "branch in a switch statement"
                }
            },
            {"break",
                new CompletionItem
                {
                    Label = "break",
                    Detail = "break Keyword",
                    Documentation = "terminates a loop or switch statement"
                }
            },
            {"return",
                new CompletionItem
                {
                    Label = "return",
                    Detail = "return Keyword",
                    Documentation = "exits a method and optionally returns a value"
                }
            },
            {"try",
                new CompletionItem
                {
                    Label = "try",
                    Detail = "try Keyword",
                    Documentation = "begins a block of code to handle exceptions"
                }
            },
            {"catch",
                new CompletionItem
                {
                    Label = "catch",
                    Detail = "catch Keyword",
                    Documentation = "handles exceptions thrown by try block"
                }
            },
            {"nil",
                new CompletionItem
                {
                    Label = "nil",
                    Detail = "nil Keyword",
                    Documentation = "represents a nil reference"
                }
            },
            {"true",
                new CompletionItem
                {
                    Label = "true",
                    Detail = "true Keyword",
                    Documentation = "boolean true value"
                }
            },
            {"false",
                new CompletionItem
                {
                    Label = "false",
                    Detail = "false Keyword",
                    Documentation = "boolean false value"
                }
            },
            {"this",
                new CompletionItem
                {
                    Label = "this",
                    Detail = "this Keyword",
                    Documentation = "refers to the current instance of the class"
                }
            },
            {"import",
                new CompletionItem
                {
                    Label = "import",
                    Detail = "import Keyword",
                    Documentation = "imports files/folders"
                }
            },
        };
        public static List<CompletionItem> TryGetKeywordCompletions(string currentWord, List<CompletionItem> items)
        {
            foreach (string key in keywords.Keys)
            {
                if (currentWord.Contains(key) || key.Contains(currentWord) || key.Contains(currentWord.Substring(0)))
                {
                    items.Add(keywords[key]);
                }
            }
            return items;
        }
    }
    
    
}
