using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScarbroScriptLSP.LSP;
using ScarbroScript;


namespace ScarbroScriptLSP.Analysis
{
    class State
    {
        public static Dictionary<string, string> documents { get; set; }


        public State()
        {
            documents = new Dictionary<string, string>();
        }


        public PublishDiagnosticNotification OpenDocument(string uri, string text)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            documents[encodedUri] = text;
            return new PublishDiagnosticNotification
            {
                Method = "textDocument/publishDiagnostics",
                RPC = "2.0",
                Params = new PublishDiagnosticParams
                {
                    URI = encodedUri,
                    Diagnostics = GetDiagnosticsFromFile(text)
                }

            };
        }

        //same thing different naming cnvention :(
        public PublishDiagnosticNotification UpdateDocument(string uri, string text)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            documents[encodedUri] = text;
            return new PublishDiagnosticNotification
            {
                Method = "textDocument/publishDiagnostics",
                RPC = "2.0",
                Params = new PublishDiagnosticParams
                {
                    URI = encodedUri,
                    Diagnostics = GetDiagnosticsFromFile(text)
                }
                
            };
        }

        public List<Diagnostic> GetDiagnosticsFromFile(string text)
        {
            // Obviously in real life we would probably pass in any Errors found from teh Parser
            // if there are no errors we would maybe pass in the AST for linting nd any type mismatches
            List<Diagnostic> diagnostics = new List<Diagnostic>();

            
            Scanner scannerT = new Scanner(text); 
            List<Token> tokens = scannerT.ScanTokens();
            LintParser parser = new LintParser(tokens);
            List<Exception> errs = parser.Parse();

            var lines = text.Split('\n');
            foreach (LintParser.LintParseError err in errs)
            {
                if (text.Contains(err.problemChild))
                {
                    string line = lines[err.line - 1];
                    Program.logger.Log("Reported Line = " + err.line + " Line In File = " + line);
                    int idx = line.IndexOf(err.problemChild);
                    Program.logger.Log("Found the problem we were looking for " + err.problemChild);
                    if (idx >= 0)
                    {
                        Program.logger.Log("Found index! " + idx);
                        diagnostics.Add(new Diagnostic
                        {

                            Range = Range.NewLineRange(err.line, idx - 1, idx + err.problemChild.Length),
                            Severity = 1,
                            Source = "ScarbroScript Parse Error",
                            Message = err.Message

                        });

                    }
                    else
                    {
                        Program.logger.Log(err.problemChild + " was not found in the text");
                    }
                }
            }

            //var lines = text.Split('\n');
                
            
            
            return diagnostics;
        }


        // in real life (the final product) this would look up the type in our type analysis code
        public HoverResponse Hover(int id, string uri, Position position)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            var document = documents[encodedUri];
            return new HoverResponse("2.0", id, $"File: {uri} Characters: {document.Length}");
        }

        public DefinitionResponse Definition(int id, string uri, Position position)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            var document = documents[encodedUri];
            var gotoURI = new Uri("C:/Users/Admin/source/repos/ScarbroScriptLSP/ScarbroScriptLSP/LSP/Message.cs").AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for Got URI: {gotoURI}");
            // Obviously this is just a dummy response we
            // would probably get the position from the request
            // and use it to search for any of OUR native mods
            // OR we would search the file to maybe look for where
            // its used and give that position 
            return new DefinitionResponse("2.0", id, gotoURI, position.Line - 1, position.Line + 1);
        }


        public CodeActionResponse CodeAction(int id, string uri, object any)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            var text = documents[encodedUri];
            var lines = text.Split('\n');
            // Code actions

            List<CodeAction> codeActions = new List<CodeAction>();
            
            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row];
                int i = line.IndexOf("file");
                Program.logger.Log("Making Code Action");
                if (i >= 0)
                {
                    Program.logger.Log("Found index! " + i);
                    codeActions.Add(new CodeAction
                    {
                        Title = "Replace File with Poopy!",
                        Edit = new WorkspaceEdit
                        {
                            Changes = new Dictionary<string, List<TextEdit>>
                            {
                                [encodedUri] = new List<TextEdit>
                                {
                                    new TextEdit
                                    {
                                        Range = Range.NewLineRange(row, i, i + "file".Length),
                                        NewText = "Poopy"
                                    }
                                    
                                }
                            }
                        }

                    });
                    Program.logger.Log("just Added a new code action");
                }
                else
                {
                    Program.logger.Log("could not find index");
                }
            }
            return new CodeActionResponse("2.0", id, uri, codeActions);
        }


        public CompletionResponse Completion(int id, string uri, Position position)
        {
            var encodedUri = new Uri(uri).AbsoluteUri;
            Program.logger.Log($"Handling CodeAction for URI: {encodedUri}");
            var document = documents[encodedUri];
            var items = new List<CompletionItem>();
            items.Add(new CompletionItem
            {
                Label = "Just a Dummy AutoComplete",
                Detail = "Testing 123",
                Documentation = "ScarbroScriptLSP is running the autoComplete..."
            });

            //int the actual ask your static analysis tools to figure out good completions
            return new CompletionResponse("2.0", id, items);
        }

        
    }
}
