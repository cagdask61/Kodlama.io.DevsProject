using Core.CrossCuttingConcers.Logging.Serilog.ConfigurationModels;
using Core.CrossCuttingConcers.Logging.Serilog.Messages;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcers.Logging.Serilog.Logger
{
    public class FileLogger : LoggerServiceBase
    {
        private IConfiguration Configuration { get; set; }

        public FileLogger(IConfiguration configuration)
        {
            Configuration = configuration;

            FileLogConfiguration logConfiguration = configuration.GetSection("SerilogConfiguration:FileLogConfiguration").Get<FileLogConfiguration>() ?? throw new Exception(SerilogMessages.NullOptionsMessage);


            string logFilePath = string.Format("{0},{1}",Directory.GetCurrentDirectory() + logConfiguration.FolderPath, ".txt");

            Logger = new LoggerConfiguration()
                .WriteTo
                .File(
                logFilePath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: null,
                fileSizeLimitBytes: 5000000,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{level}] {Message}{NewLine}{Exception}")
                .CreateLogger();

        }


        
    }
}
