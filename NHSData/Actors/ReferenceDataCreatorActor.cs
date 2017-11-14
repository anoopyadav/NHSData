using Akka.Actor;
using Akka.Event;
using NHSData.CsvMaps;
using NHSData.DataObjects;
using NHSData.Messages;
using NHSData.ReferenceData;

namespace NHSData.Actors
{
    public class ReferenceDataCreatorActor : ReceiveActor
    {
        private readonly IActorRef _postcodeCsvReader;
        private readonly ILoggingAdapter _logger;
        private readonly IReferenceDataWriter _postcodeReferenceDataWriter;
        public ReferenceDataCreatorActor(string sourcePath)
        {
            _postcodeCsvReader = Context.ActorOf(Props.Create<CsvReaderActor<PostcodeRow, PostcodeMap>>(sourcePath));
            _postcodeReferenceDataWriter = new PostcodeReferenceDataWriter();
            _logger = Context.GetLogger();

            Receive<InitiateAnalysisMessage>(message =>
            {
                _logger.Info($"Received {nameof(message)}, proceeding with Reference Data generation.");
                _postcodeCsvReader.Tell(new InitiateAnalysisMessage());
            });

            Receive<DataRowMessage>(message => PopulateReferenceData(message));

            Receive<FileAnalysisFinishedMessage>(message => WriteReferenceData());
        }

        private void PopulateReferenceData(DataRowMessage message)
        {
            _postcodeReferenceDataWriter.UpdateReferenceData(message.Row);
        }

        private void WriteReferenceData()
        {
            _logger.Info("Received FileAnalysisFinishedMessage, commiting changes");
            _postcodeReferenceDataWriter.WriteReferenceData();
            _logger.Info("Finished writing Postcode Reference Data.");
            Context.Parent.Tell(new FileAnalysisFinishedMessage());
        }
    }
}