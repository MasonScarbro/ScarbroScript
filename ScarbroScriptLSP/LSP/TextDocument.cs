using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ScarbroScriptLSP.LSP
{

    #region Global
    // For Better Context these are all used amongst the different textdoc stuff
    public class Position
    {
        [JsonPropertyName("line")]
        public int Line { get; set; }

        [JsonPropertyName("character")]
        public int Character { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("uri")]
        public string URI { get; set; }

        [JsonPropertyName("range")]
        public Range Range { get; set; }
    }

    public class Range
    {
        public static Range NewLineRange(int line, int start, int end)
        {
            return new Range
            {
                Start = new Position
                {
                    Line = line,
                    Character = start
                },
                End = new Position
                {
                    Line = line,
                    Character = end
                }
            };
        }

        [JsonPropertyName("start")]
        public Position Start { get; set; }

        [JsonPropertyName("end")]
        public Position End { get; set; }


    }

    public class Diagnostic
    {
        [JsonPropertyName("range")]
        public Range Range { get; set; }

        [JsonPropertyName("severity")]
        public int Severity { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class WorkspaceEdit
    {
        [JsonPropertyName("changes")]
        public Dictionary<string, List<TextEdit>> Changes { get; set; }
    }
    
    public class TextEdit
    {
        [JsonPropertyName("range")]
        public Range Range { get; set; }

        [JsonPropertyName("newText")]
        public string NewText { get; set; }
    }

    public class TextDocumentPositionParams
    {
        [JsonPropertyName("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonPropertyName("position")]
        public Position Position { get; set; }
    }

    public class TextDocumentItem
    {

        [JsonPropertyName("uri")]
        public string URI { get; set; }

        [JsonPropertyName("languageid")]
        public string LanguageId { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

    }

    #endregion
    #region DidOpen
    public class TextDocumentDidOpen : Notification
    {
        [JsonPropertyName("params")]
        public DidOpenParams Params { get; set; }
    }

    public class DidOpenParams
    {
        [JsonPropertyName("textDocument")]
        public TextDocumentItem TextDocument { get; set; }
    }
    #endregion

    #region DidChange

    public class TextDocumentDidChange : Notification
    {
        [JsonPropertyName("params")]
        public DidChangeParams Params { get; set; }
    }

    public class TextDocumentIdentifier
    {
        [JsonPropertyName("uri")]
        public string URI { get; set; }
    }

    public class VersionedTextDocumentIdentifier : TextDocumentIdentifier
    {
        [JsonPropertyName("version")]
        public int Version { get; set; }
    }

    public class TextDocumentContentChangeEvent
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }

    public class DidChangeParams
    {
        [JsonPropertyName("textDocument")]
        public VersionedTextDocumentIdentifier TextDocument { get; set; }

        [JsonPropertyName("contentChanges")]
        public List<TextDocumentContentChangeEvent> ContentChanges { get; set; }
    }

    #endregion

    #region Hover
    //REQUEST
    public class HoverRequest : Request
    {
        [JsonPropertyName("params")]
        public HoverParams Params { get; set; }
    }





    public class HoverParams : TextDocumentPositionParams
    {

    }

    //RESPONSE

    public class HoverResponse : Response
    {


        public HoverResponse(string jsonrpc, int id, string contents) : base(jsonrpc, id)
        {
            SetResult(Result = new HoverResult
            {
                Contents = contents
            }); ;
        }


        [JsonPropertyName("result")]
        HoverResult Result { get; set; }
    }

    public class HoverResult
    {
        [JsonPropertyName("contents")]
        public string Contents { get; set; }
    }

    #endregion

    #region GoTo
    public class DefinitionRequest : Request
    {
        [JsonPropertyName("params")]
        public DefinitionParams Params { get; set; }
    }

    public class DefinitionParams : TextDocumentPositionParams
    {

    }

    //RESPONSE

    public class DefinitionResponse : Response
    {


        public DefinitionResponse(string jsonrpc, int id, string uri, int lineStart, int lineEnd) : base(jsonrpc, id)
        {

            SetResult(Result = new Location
            {
                URI = uri,
                Range = new Range
                {
                    Start = new Position
                    {
                        Line = lineStart,
                        Character = 0
                    },
                    End = new Position
                    {

                        Line = lineEnd,
                        Character = 0
                    }
                }
            });

            Program.logger.Log($"Result Range Start Line: {Result.Range.Start.Line}, Character: {Result.Range.Start.Character}");
        }


        [JsonPropertyName("result")]
        Location Result { get; set; }
    }



    #endregion

    #region CodeAction

    //REQUEST:
    public class CodeActionRequest : Request
    {
        [JsonPropertyName("params")]
        public CodeActionParams Params { get; set; }
    }

    public class CodeActionParams
    {
        [JsonPropertyName("textDocument")]
        public TextDocumentIdentifier TextDocument { get; set; }

        [JsonPropertyName("range")]
        public Range Range { get; set; }

        [JsonPropertyName("context")]
        public CodeActionContext Context { get; set; }
    }

    public class CodeActionContext
    {
        [JsonPropertyName("diagnostics")]
        public List<Diagnostic> Diagnostics { get; set; } = new List<Diagnostic>();

        [JsonPropertyName("only")]
        public List<string> Only { get; set; } = new List<string>();

        [JsonPropertyName("triggerKind")]
        public int TriggerKind { get; set; }
    }



    //RESPONSE:

    public class CodeActionResponse : Response
    {
        public CodeActionResponse(string jsonrpc, int id, string uri, List<CodeAction> codeActions ) : base(jsonrpc, id)
        {
            Result = codeActions;
            SetResult(Result);
        }

        [JsonPropertyName("result")]
        public List<CodeAction> Result { get; set; }
    }

    public class CodeAction
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        
        [JsonPropertyName("edit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WorkspaceEdit Edit { get; set; }


        [JsonPropertyName("command")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Command Command { get; set; }
    }

    public class Command
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("command")]
        public string Cmd { get; set; }

        [JsonPropertyName("arguments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<object> Arguments { get; set; }
    }

    #endregion

    #region Completion
    public class CompletionRequest : Request
    {
        [JsonPropertyName("params")]
        public CompletionParams Params { get; set; }
    }

    public class CompletionParams : TextDocumentPositionParams
    {

    }

    //RESPONSE

    public class CompletionResponse : Response
    {


        public CompletionResponse(string jsonrpc, int id, List<CompletionItem> items) : base(jsonrpc, id)
        {
            SetResult(Result = items);
        }


        [JsonPropertyName("result")]
        List<CompletionItem> Result { get; set; }
    }

    public class CompletionItem
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        [JsonPropertyName("documentation")]
        public string Documentation { get; set; }
    }

    #endregion

    #region DiagnosticNotification

    public class PublishDiagnosticNotification : Notification
    {


        [JsonPropertyName("params")]
        public PublishDiagnosticParams Params { get; set; }

    }

    public class PublishDiagnosticParams 
    {

        [JsonPropertyName("uri")]
        public string URI { get; set; }

        [JsonPropertyName("diagnostics")]
        public List<Diagnostic> Diagnostics { get; set; }

    }


    #endregion


}
