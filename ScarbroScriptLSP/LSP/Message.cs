using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ScarbroScriptLSP.LSP
{
    public class Request
    {
        [JsonPropertyName("jsonrpc")]
        public string RPC { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        //Params ...
    }


    public class Response
    {
        public Response(string RPC, int ID)
        {
            this.RPC = RPC;
            this.ID = ID;

        }

        [JsonPropertyName("jsonrpc")]
        public string RPC { get; set; }

        [JsonPropertyName("id")]
        public int ID { get; set; }

        // Always has a response or Error
        //Result
        //Error
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("result")]
        public object Result { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("error")]
        public object Error { get; set; }

        public void SetResult(object result)
        {
            Result = result;
            Error = null;
        }

        public void SetError(object error)
        {
            Error = error;
            Result = null;
        }
    }

    public class Notification
    {

        [JsonPropertyName("jsonrpc")]
        public string RPC { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        // Always has a response or Error
        //Result
        //Error
    }

}
