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
        private Linterpreter.Scoper scoper = new Linterpreter.Scoper();

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
            LintParser errParser = new LintParser(tokens);
            List<Exception> errs = errParser.Parse();
            if (errs == null || errs.Count == 0)
            {
                Parser parser = new Parser(tokens);
                List<Stmt> statements = parser.Parse();
                scoper = new Linterpreter.Scoper(statements);
                List<Exception> linterpreterErrors = scoper.GetLinterpreterErrors();
                if (linterpreterErrors.Count != 0)
                {
                    errs = linterpreterErrors;
                }
            }

            var lines = text.Split('\n');

            
            foreach (var error in errs)
            {
                if (error is LintParser.LintParseError lintError)
                {
                    ProcessLintError(lintError, text, lines, diagnostics);
                }
                else if (error is Linterpreter.LinterpreterError linterpreterError)
                {
                    ProcessLinterpreterError(linterpreterError, text, lines, diagnostics);
                }
            }

            //var lines = text.Split('\n');



            return diagnostics;
        }

        private void ProcessLintError(LintParser.LintParseError err, string text, string[] lines, List<Diagnostic> diagnostics)
        {
            Program.logger.Log($"Processing error: {err.Message} at line {err.line}, problem: {err.problemChild}");
            if (text.Contains(err.problemChild))
            {
                if (err.line <= 0 || err.line > lines.Length)
                {
                    Program.logger.Log("err.line was not greater than 0 or was <= lines.length");
                    return;
                }
                string line = lines[err.line - 1];
                Program.logger.Log("Reported Line = " + err.line + " Line In File = " + line);
                int idx = line.IndexOf(err.problemChild);
                Program.logger.Log("Found the problem we were looking for " + err.problemChild);

                Program.logger.Log($"Line {err.line}: {line}");
                Program.logger.Log($"Index of problemChild '{err.problemChild}': {idx}");

                if (idx >= 0)
                {
                    Program.logger.Log("Found index! " + idx);
                    diagnostics.Add(new Diagnostic
                    {
                        Range = Range.NewLineRange(err.line - 1, idx, idx + err.problemChild.Length),
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

        private void ProcessLinterpreterError(Linterpreter.LinterpreterError err, string text, string[] lines, List<Diagnostic> diagnostics)
        {
            Program.logger.Log($"Processing error: {err.Message} at line {err.line}, problem: {err.problemChild}");
            if (text.Contains(err.problemChild))
            {
                if (err.line <= 0 || err.line > lines.Length)
                {
                    Program.logger.Log("err.line was not greater than 0 or was <= lines.length");
                    return;
                }
                string line = lines[err.line - 1];
                Program.logger.Log("Reported Line = " + err.line + " Line In File = " + line);
                int idx = line.IndexOf(err.problemChild);
                Program.logger.Log("Found the problem we were looking for " + err.problemChild);

                Program.logger.Log($"Line {err.line}: {line}");
                Program.logger.Log($"Index of problemChild '{err.problemChild}': {idx}");

                if (idx >= 0)
                {
                    Program.logger.Log("Found index! " + idx);
                    diagnostics.Add(new Diagnostic
                    {
                        Range = Range.NewLineRange(err.line - 1, idx, idx + err.problemChild.Length),
                        Severity = 1,
                        Source = "ScarbroScript Potential Runtime Error",
                        Message = err.Message
                    });
                }
                else
                {
                    Program.logger.Log(err.problemChild + " was not found in the text");
                }
            }
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
            Program.logger.Log($"Handling Completion for URI: {encodedUri}");
            var document = documents[encodedUri];
            var items = new List<CompletionItem>();
            var lines = document.Split('\n');

            if (position.Line < lines.Length)
            {
                string line = lines[position.Line];
                if (position.Character <= line.Length)
                {
                    
                    items = GetClassAutoCompletion(line, position, Linterpreter.scopedVariables);
                    
                }
            }

            

            //int the actual ask your static analysis tools to figure out good completions
            return new CompletionResponse("2.0", id, items);
        }

        private List<CompletionItem> GetClassAutoCompletion(string line, Position position, Dictionary<string, object> scopedobjs)
        {
            string beforeCursor = line.Substring(0, position.Character);
            int lastDotIndex = beforeCursor.LastIndexOf('.');

            if (lastDotIndex > 0)
            {
                string wordBeforeDot = beforeCursor.Substring(0, lastDotIndex).Trim().Split().Last();
                Program.logger.Log($"Word before '.': {wordBeforeDot}");
                if (Linterpreter.lintErrors != null)
                {
                    Program.logger.Log("found a Linter Err (will be handled in diagnostics later)");
                }
                if (!Linterpreter.scopedVariables.ContainsKey(wordBeforeDot))
                {
                    Program.logger.Log("Variable Not Found in scope");
                }
                if (Linterpreter.scopedVariables.ContainsKey(wordBeforeDot))
                {
                    Program.logger.Log("Variable Found in scope");
                    return NativeClassChecker.TryGetNatives(Linterpreter.scopedVariables[wordBeforeDot]);
                }
                return NativeClassChecker.TryGetNatives(wordBeforeDot);
                // This would be for classes built by the user
                //if (wordBeforeDot == nonNativeClass)
                //{
                //    var methods = new List<string> { };
                //    //... add each method based on what was found
                //    var items = methods.Select(func => new CompletionItem
                //    {
                //        Label = func,
                //        Detail = "Math Function",
                //        Documentation = $"Math.{func}() function"
                //    }).ToList();

                //    return new CompletionResponse("2.0", id, items);
                //}
            }
            Program.logger.Log($"Err: lastDotIndex was < 0");
            return null;
        }

        
    }
}
