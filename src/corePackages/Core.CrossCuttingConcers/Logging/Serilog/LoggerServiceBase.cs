using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Logging.Serilog
{
    public abstract class LoggerServiceBase
    {
        protected ILogger Logger { get; set; }


        public void Verbose(string message) => Logger.Verbose(message);

        public void Fatal(string message) => Logger.Fatal(message);

        public void Information(string message) => Logger.Information(message);

        public void Warning(string message) => Logger.Warning(message);

        public void Debug(string message) => Logger.Debug(message);

        public void Error(string message) => Logger.Error(message);
    }
}
