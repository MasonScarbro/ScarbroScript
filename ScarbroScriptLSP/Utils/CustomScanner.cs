using System;
using System.IO;
using System.Text;

namespace ScarbroScriptLSP.Utils
{
    public class CustomScanner
    {
        private readonly Stream stream;
        //private readonly BufferedStream bfs;
        private byte[] buffer = new byte[8192];  // Adjust the buffer size as needed
        private int currentIndex = 0;
        private int bufferLength = 0;
        private byte[] currentToken = null;

        public CustomScanner(Stream inputStream)
        {
            stream = inputStream;
           
            FillBuffer();
        }

        public bool Scan()
        {
            Program.logger.Log("We are in the Scan process");
            while (true)
            {
                try
                {
                    if (currentIndex >= bufferLength)
                    {
                        FillBuffer();
                    }

                    if (bufferLength == 0)
                    {
                        return false; // No more data to read
                    }

                    Program.logger.Log("Before Running Split the current index was: " + currentIndex);
                    Program.logger.Log("Before Running Split the bufferLength was: " + bufferLength);
                    (int advance, byte[] token) = Rpc.Split(buffer.AsSpan(currentIndex, bufferLength - currentIndex).ToArray(), true);
                    Program.logger.Log("Advance Was: " + advance);
                    Program.logger.Log("And the Token Was: " + Encoding.UTF8.GetString(token));
                    if (advance > 0 && token != null && token.Length > 0)
                    {
                        Program.logger.Log("We Just Finished the Split Func INSIDE Scan and Token is: \n" + Encoding.UTF8.GetString(token) + " \nAnd the Advance is: " + advance);
                        currentToken = token;
                        currentIndex += advance;
                        return true;
                    }
                    else
                    {
                        Program.logger.Log("No valid token found or token length is zero. Attempting to fill buffer.");
                        FillBuffer();
                    }
                }
                catch (Exception e)
                {
                    Program.logger.Log("Ran Into a Problem trying to access the Split function ERR: " + e.Message);
                    return false;
                }
            }
        }

        public byte[] Bytes()
        {
            // Return the current token/message
            Program.logger.Log("We Made it to the Bytes process");
            return currentToken;
        }

        private void FillBuffer()
        {
            if (stream.CanRead)
            {
                try
                {
                    if (currentIndex < bufferLength)
                    {
                        Array.Copy(buffer, currentIndex, buffer, 0, bufferLength - currentIndex);
                        bufferLength -= currentIndex;
                    }
                    else
                    {
                        bufferLength = 0;
                    }
                    int bytesRead = stream.Read(buffer, bufferLength, buffer.Length - bufferLength);
                    if (bytesRead > 0)
                    {
                        bufferLength += bytesRead;
                        currentIndex = 0;
                        Program.logger.Log("Buffer filled with " + bufferLength + " bytes.");
                    }
                }
                catch (Exception e)
                {
                    Program.logger.Log("Error filling buffer: " + e.Message);
                }
            }
        }
    }
}
