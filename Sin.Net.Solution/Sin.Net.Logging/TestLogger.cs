﻿using NLog;
using NLog.Layouts;
using Sin.Net.Domain.Persistence.Logging;
using System;

namespace Sin.Net.Logging
{
    public class TestLogger : ILoggable
    {
        // -- fields

        private Logger _logger;

        // -- constructor

        public TestLogger()
        {
            Start();
        }

        // -- methods

        #region Start
        public void Start()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var consoleTarget = new NLog.Targets.ConsoleTarget("logconsole");

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

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, consoleTarget);

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
