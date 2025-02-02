﻿using NLog;
using NLog.Layouts;
using Sin.Net.Domain.Persistence.Logging;
using System;
using System.IO;

namespace Sin.Net.Logging
{
    public class NLogger : ILoggable
    {
        // -- fields

        private Logger _logger;

        private LogLevel _minLevel;

        // -- constructors

        public NLogger(bool deleteOldFiles = true)
        {
            Start();
        }

        public NLogger(LogLevel minLevel, bool deleteOldFiles = true) : this(deleteOldFiles)
        {
            _minLevel = minLevel;
            Start();
        }

        // -- methods

        #region Start
        public void Start()
        {
            var path = "Logs/";
            var config = new NLog.Config.LoggingConfiguration();
            var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");
            var fileTarget = new NLog.Targets.FileTarget("logfile");
            var filename = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
#if DEBUG
            var suffix = "_DEBUG";
            var minRule = LogLevel.Debug;
#else
            var suffix = "";
            var minRule = LogLevel.Info;
#endif
            consoleTarget.Layout = new CsvLayout()
            {
                Columns = {
                    new CsvColumn("Time", "${date:format=yyyy-MM-dd HH\\:mm\\:ss}"),
                    new CsvColumn("Level", "${level}"),
                    new CsvColumn("Message", "${message}"),
                },
                Delimiter = CsvColumnDelimiterMode.Custom,
                CustomColumnDelimiter = " | ",
                Quoting = CsvQuotingMode.Nothing,
            };

            fileTarget.Layout = new CsvLayout()
            {
                Columns = {
                    new CsvColumn("Time", "${date:format=yyyy-MM-dd HH\\:mm\\:ss}"),
                    new CsvColumn("Level", "${level}"),
                    new CsvColumn("Message", "${message}"),
                },
                Delimiter = CsvColumnDelimiterMode.Tab
            };

            fileTarget.FileName = $"{Path.Combine(path, filename)}{suffix}.log";
            fileTarget.DeleteOldFileOnStartup = true;
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);
            config.AddRule(minRule, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }
        #endregion

        public void Stop()
        {
            NLog.LogManager.Shutdown();
        }

        // -- log methods

        public void Trace(string msg)
        {
            _logger.Trace(msg);
        }

        public void Debug(string msg)
        {
            _logger.Debug(msg);
        }

        public void Info(string msg)
        {
            _logger.Info(msg);
        }

        public void Warn(string msg)
        {
            _logger.Warn(msg);
        }

        public void Error(string msg)
        {
            _logger.Error(msg);
        }

        public void Fatal(Exception ex)
        {
            _logger.Fatal(ex);
        }
    }
}