using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using ScarbroScriptLSP.Utils;


namespace ScarbroScriptLSP
{
    public class Rpc
    {

        public static object EncodeMessage(object message)
        {
            try
            {

                string content = JsonSerializer.Serialize(message);
                Program.logger.Log("Serialized Content: " + content);

                int contentLength = Encoding.UTF8.GetByteCount(content); // Use byte count for accurate content length
                string header = $"Content-Length: {contentLength}\r\n\r\n";

                Program.logger.Log("Header: " + header);

                string encodedMessage = header + content;

                Program.logger.Log("Encoded Message: " + encodedMessage);

                return encodedMessage;


            }
            catch (Exception e)
            {
                Program.logger.Log("Error: " + e.Message);
            }
            return null;
            

        }

        public static (byte[] contentBytes, string) DecodeMessage(byte[] content)
        {
            string headerStr = Encoding.UTF8.GetString(content);
            const string contentLengthPrefix = "Content-Length: ";
            Program.logger.Log("We Entered the Decode Message Func and the headerStr is: " + headerStr);

            // Find Content-Length prefix in header
            int prefixIndex = headerStr.IndexOf(contentLengthPrefix);

            if (prefixIndex == -1)
            {
                throw new ArgumentException("Invalid header format: Content-Length prefix not found.");
            }

            // Calculate start index for Content-Length value
            int startIndex = prefixIndex + contentLengthPrefix.Length;
            // Find end of line after Content-Length value
            int endIndex = headerStr.IndexOf("\r\n\r\n", startIndex);

            if (endIndex == -1)
            {
                throw new ArgumentException("Invalid header format: End of line not found after Content-Length.");
            }

            //Get Content-Length as a string and parse it to int
            string contentLengthStr = headerStr.Substring(startIndex, endIndex - startIndex).Trim();
            Program.logger.Log("The contentLengthStr is: " + contentLengthStr);
            if (!int.TryParse(contentLengthStr, out int contentLength))
            {
                throw new ArgumentException("Invalid header format: Content-Length value is not a valid integer.");
            }

            // Check if content length exceeds actual content size
            if (contentLength > content.Length)
            {
                throw new ArgumentException("Invalid content length: Content-Length exceeds actual content size.");
            }

            int jsonStartIndex = endIndex + 4;//after \r\n\r\n
            // Slice content array based on content length
            byte[] contentBytes = new byte[contentLength];
            Array.Copy(content, jsonStartIndex, contentBytes, 0, contentLength);
            //Console.WriteLine(Encoding.UTF8.GetString(contentBytes));
            //define
            BaseMessage baseMessage = null;

            try
            {
                baseMessage = JsonSerializer.Deserialize<BaseMessage>(contentBytes);
            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                throw new ArgumentException("Error deserializing JSON content.");
            }
            //Program.logger.Log("The contentBytes is: " + Encoding.UTF8.GetString(contentBytes));
            //Console.WriteLine(baseMessage.Method);
            return (contentBytes, baseMessage.Method);

        }

        public static (int advance, byte[] token) Split(byte[] data, bool _)
        {
            Program.logger.Log("We are in the Split process");
            Program.logger.Log("And The Data was: " + Encoding.UTF8.GetString(data));
            byte[] delimiter = Encoding.UTF8.GetBytes("\r\n\r\n");
            if (!ByteUtils.Cut(data, delimiter, out byte[] header, out byte[] content))
            {
                Program.logger.Log("Delimiter not found in data");
                // Handle case where delimiter is not found
                return (0, Array.Empty<byte>());
            }
            
            try
            {
                string headerStr = Encoding.UTF8.GetString(header);
                Program.logger.Log("Okay The Header is " + headerStr);
                const string contentLengthPrefix = "Content-Length: ";


                // Find Content-Length prefix in header
                int prefixIndex = headerStr.IndexOf(contentLengthPrefix);
                if (prefixIndex == -1)
                {
                    Program.logger.Log("Error with prefix index");
                    throw new ArgumentException("Invalid header format: Content-Length prefix not found.");
                }

                // Calculate start index for Content-Length value
                int startIndex = prefixIndex + contentLengthPrefix.Length;

                // Extract Content-Length value until the end of the header
                string contentLengthStr = headerStr.Substring(startIndex).Trim();
                if (!int.TryParse(contentLengthStr, out int contentLength))
                {
                    Program.logger.Log("Error trying to parse contentLengthStr");
                    throw new ArgumentException("Invalid header format: Content-Length value is not a valid integer.");
                }

                if (content == null)
                {
                    Program.logger.Log("Content is null");
                    content = Array.Empty<byte>();
                }
 
                Program.logger.Log("contentLengthStr was: " + contentLengthStr);
                Program.logger.Log("Content was: " + Encoding.UTF8.GetString(content));
                Program.logger.Log("content.Length was: " + content.Length + " Vs " + "contentLength: " + contentLength);
                if (content.Length < contentLength)
                {
                    
                    Program.logger.Log("The content.Length < contentLength");
                    return (0, Array.Empty<byte>());
                }
                Program.logger.Log("The Data Thus Far in The Split Function is: " + Encoding.UTF8.GetString(data));
                int totalLength = header.Length + delimiter.Length + contentLength;
                Program.logger.Log("Okay The Total Length in Bytes is: " + totalLength + " And the Token returned is: " + Encoding.UTF8.GetString(data.Take(totalLength).ToArray()));
                return (totalLength, data.Take(totalLength).ToArray());

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                throw new ArgumentException("Content-Length NaN");
            }
        }

    }


    public class BaseMessage
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }
    }
}
