using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ScarbroScriptLSP.LSP
{
    public class InitializeRequest : Request
    {

        [JsonPropertyName("params")]
        public InitializeRequestParams Params { get; set; }

    }

    public class InitializeRequestParams
    {
        [JsonPropertyName("clientInfo")]
        public ClientInfo ClientInfo { get; set; }

    }

    public class ClientInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }


    public class InitializeResponse : Response
    {
        

        public InitializeResponse(string jsonrpc, int id) : base(jsonrpc, id)
        {
            
            SetResult(Result = new InitializeResult
            {
                Capabilities = new ServerCapabilities
                {
                   TextDocumentSync = 1,
                   HoverProvider = true,
                   DefinitionProvider = true,
                   CodeActionProvider = true,
                   CompletionProvider = new CompletionOptions
                   {
                        ResolveProvider = true,
                        TriggerCharacters = new List<string> { ".", ",", "a", "b", "c", "d", "e", "f", "g", "h", "i"} // Add appropriate trigger characters
                   }

                },
                Info = new ServerInfo
                {
                    Name = "ScarbroScriptLSP",
                    Version = "0.0.1"
                }
            });
        }

        private InitializeResult NewInitializeResponse(string id)
        {
            var result = new InitializeResult
            {
                Capabilities = null,
                Info = new ServerInfo
                {
                    Name = "ScarbroScriptLSP",
                    Version = "0.0.1"
                }
            };
            return result;
        }
    }

    public class InitializeResult
    {
        [JsonPropertyName("capabilities")]
        public ServerCapabilities Capabilities { get; set; }

        [JsonPropertyName("serverInfo")]
        public ServerInfo Info { get; set; }
    }

    public class ServerCapabilities
    {

        [JsonPropertyName("textDocumentSync")]
        public int TextDocumentSync { get; set; }

        [JsonPropertyName("hoverProvider")]
        public bool HoverProvider { get; set; }

        [JsonPropertyName("definitionProvider")]
        public bool DefinitionProvider { get; set; }

        [JsonPropertyName("codeActionProvider")]
        public bool CodeActionProvider { get; set; }

        [JsonPropertyName("completionProvider")]
        public CompletionOptions CompletionProvider { get; set; }

    }

    public class CompletionOptions
    {
        [JsonPropertyName("resolveProvider")]
        public bool ResolveProvider { get; set; }

        [JsonPropertyName("triggerCharacters")]
        public List<string> TriggerCharacters { get; set; }
    }

    public class ServerInfo
    {

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

    }


    
}
