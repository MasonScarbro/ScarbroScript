using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ScarbroScriptLSP.Utils;
using System.Diagnostics;
using System.Threading;
using System.Text.Json.Serialization;
using System.Text.Json;
using ScarbroScriptLSP.LSP;
using ScarbroScriptLSP.Analysis;
using ScarbroScript;


namespace ScarbroScriptLSP
{
    
    class Program
    {
        public static Logger logger;

        static void Main(string[] args)
        {

            if (Debugger.IsAttached) Debugger.Launch();

            


            logger = GetLogger("C:/Users/Admin/Desktop/test-log.txt");
            logger.Log("Hey I started");
            // byte[] incomingMsg = Encoding.UTF8.GetBytes("Content-Length: 15\r\n\r\n{\"Method\":\"hi\"}");
            // Read input from stdin
            var state = new State();
            
            using (StreamReader reader = new StreamReader(Console.OpenStandardInput()))
            {
                logger.Log("We Opened Stdout");
                CustomScanner scanner = new CustomScanner(reader.BaseStream);
                logger.Log("Okay We Just Made It Past Custom Scanner");
                while (scanner.Scan())
                {
                    logger.Log("Currently Scanning!!!");
                    byte[] msg = scanner.Bytes();
                    if (msg != null)
                    {
                        logger.Log("The MSG  was not null");
                        //string message = System.Text.Encoding.UTF8.GetString(msg);
                        HandleMessage(logger, state, msg);
                    }
                    else
                    {
                        logger.Log("Msg Was Null I.e it failed :(");
                    }
                }
            }

        }

        


        private static void HandleMessage(Logger logger, State state, byte[] message)
        {
            logger.Log("The Message Is Complete Here it is: " + System.Text.Encoding.UTF8.GetString(message));
            try
            {
                (byte[] content, string method) = Rpc.DecodeMessage(message);
                logger.Log("We Just Recieved Method: " + method);


                switch (method)
                {
                    case "initialize":
                     
                        try
                        {
                            var request = JsonSerializer.Deserialize<InitializeRequest>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"ClientInfo Name: {request.Params.ClientInfo.Name}");
                                logger.Log($"ClientInfo Version: {request.Params.ClientInfo.Version}");
                                InitializeResponse msg = new InitializeResponse("2.0", request.ID);
                                WriteResponse(msg);
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Deserialize: " + e.Message);
                        
                        }
                        break;
                    case "textDocument/didOpen":
                        try
                        {
                            var request = JsonSerializer.Deserialize<TextDocumentDidOpen>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");
                                logger.Log($"Contents: {request.Params.TextDocument.Text}");
                                var diagnostics = state.OpenDocument(request.Params.TextDocument.URI, request.Params.TextDocument.Text);
                                WriteResponse(diagnostics);
                                //InitializeResponse msg = new InitializeResponse("2.0", request.ID);
                                //var reply = Rpc.EncodeMessage(msg);
                                //logger.Log("Replying With: " + reply.ToString());
                                //Console.WriteLine(reply);
                                //logger.Log("Replied!");
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Deserialize DidOpen: " + e.Message);

                        }
                        break;
                    case "textDocument/didChange":
                        try
                        {
                            var request = JsonSerializer.Deserialize<TextDocumentDidChange>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");
                                
                                foreach (TextDocumentContentChangeEvent contentChange in request.Params.ContentChanges)
                                {
                                    logger.Log($"Changed: {contentChange}");
                                    var diagnostics = state.UpdateDocument(request.Params.TextDocument.URI, contentChange.Text);
                                    WriteResponse(diagnostics);
                                }
                                

                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Deserialize DidChange: " + e.Message);

                        }
                        break;
                    case "textDocument/hover":
                        try
                        {
                            var request = JsonSerializer.Deserialize<HoverRequest>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");

                                var response = state.Hover(request.ID, request.Params.TextDocument.URI, request.Params.Position);
                                WriteResponse(response);
                                
                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Deserialize Hover: " + e.Message);

                        }
                        break;
                    case "textDocument/definition":
                        try
                        {
                            var request = JsonSerializer.Deserialize<DefinitionRequest>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");

                                var response = state.Definition(request.ID, request.Params.TextDocument.URI, request.Params.Position);
                                WriteResponse(response);

                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Deserialize Definition: " + e.Message);

                        }
                        break;
                    case "textDocument/codeAction":
                        try
                        {
                            var request = JsonSerializer.Deserialize<CodeActionRequest>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");

                                var response = state.CodeAction(request.ID, request.Params.TextDocument.URI, null);
                                WriteResponse(response);

                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Handle CodeAction: " + e.Message + "\n" + e.StackTrace);

                        }
                        break;
                    case "textDocument/completion":
                        try
                        {
                            var request = JsonSerializer.Deserialize<CompletionRequest>(content);
                            if (request != null)
                            {
                                logger.Log("Deserialization successful!");
                                logger.Log($"Opened: {request.Params.TextDocument.URI}");

                                var response = state.Completion(request.ID, request.Params.TextDocument.URI, request.Params.Position);
                                WriteResponse(response);

                            }
                        }
                        catch (Exception e)
                        {
                            logger.Log("Err Trying to Handle CodeAction: " + e.Message + "\n" + e.StackTrace);

                        }
                        break;
                }
            }
            catch (Exception e)
            {
                logger.Log("Oi We Got an Error On our End! Surley Vscode is Taking the Piss Mate!");
            }
            //return null;
        }

        private static void WriteResponse(object msg)
        {
            var reply = Rpc.EncodeMessage(msg);
            logger.Log("Replying With: " + reply.ToString());
            Console.Write(reply);
            logger.Log("Replied!");
            Console.Out.Flush();
            
        }
        private static Logger GetLogger(string filename)
        {
            FileStream logFile = new FileStream(filename, FileMode.Create, FileAccess.Write);
            StreamWriter logWriter = new StreamWriter(logFile);

            logWriter.AutoFlush = true;
            Logger log = new Logger(logWriter, "[ScarbroScriptLSP]");
            return log;
        }

        

    }
}
