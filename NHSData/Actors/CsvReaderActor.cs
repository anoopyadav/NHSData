using System.IO;
using Akka.Actor;
using Akka.Event;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.DataObjects;
using NHSData.Messages;

namespace NHSData.Actors
{
    public class CsvReaderActor<TRowType, TRowMap> : ReceiveActor
    {
        private readonly ICsvReader _csvReader;
        private readonly ILoggingAdapter _logger;
        private readonly string _sourcePath;

        public CsvReaderActor(string sourcePath)
        {
            _logger = Context.GetLogger();
            _sourcePath = sourcePath;

            var configuration = new CsvConfiguration();
            configuration.RegisterClassMap(typeof(TRowMap));
            _csvReader = new CsvReader(new StreamReader(_sourcePath), configuration);

            Receive<InitiateAnalysisMessage>(message =>
            {
                _logger.Info($"Received Initiate message, commencing read for {sourcePath}");
                ParseCsvFile();
            });
        }

        public void ParseCsvFile()
        {
            while (_csvReader.Read())
            {
                var record = _csvReader.GetRecord<TRowType>() as IDataRow;
                var dataRowMessage = new DataRowMessage(typeof(TRowType), record);
                Sender.Tell(dataRowMessage, Self);
            }

            Sender.Tell(new FileAnalysisFinishedMessage());
            _logger.Info($"Finished parsing {_sourcePath}");
        }
    }
}