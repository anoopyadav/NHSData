using System;
using System.CodeDom;
using System.Data;
using Akka.Actor;
using Akka.Event;
using CsvHelper;
using NHSData.Common;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;

namespace NHSData.Actors
{
    public abstract class BaseDataAnalysisActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger;
        private readonly IDataAnalyzer _analyzer;
        private readonly IConfiguration _configuration;
        private readonly ICsvReader _csvReader;

        protected BaseDataAnalysisActor(IDataAnalyzer analyzer, IConfiguration configuration)
        {
            _analyzer = analyzer;
            _configuration =  configuration;
            _csvReader = configuration.Reader;
            _logger = Context.GetLogger();
            Stopped();
        }

        private void Stopped()
        {
            Receive<InitiateAnalysisMessage>(message =>
            {
                _logger.Info("Received InitiateAnalysisMessage, Proceeding with file analysis.");
                Become(Reading);
            });
        }

        protected abstract void Reading();

        protected abstract void Done();
    }
}