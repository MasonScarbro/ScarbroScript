using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ScarbroScriptLSP.Utils
{
    class Logger
    {

        private readonly StreamWriter _logWriter;
        private readonly string _logPrefix;

        public Logger(StreamWriter logWriter, string logPrefix)
        {
            _logWriter = logWriter;
            _logPrefix = logPrefix;
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {_logPrefix} {message}";
            _logWriter.WriteLine(logMessage);
            //Console.WriteLine(logMessage);
            
        }

    }
}
