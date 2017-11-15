using System.Configuration;
using System.IO;
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
        private readonly IActorRef _postcodeCsvReaderActor;
        private IActorRef _addressReferenceDataReaderActor;
        private readonly ILoggingAdapter _logger;
        private readonly IReferenceDataWriter _postcodeReferenceDataWriter;
        private readonly IReferenceDataWriter _locationReferenceDataWriter;
        public ReferenceDataCreatorActor(string postcodeSourcePath)
        {
            _postcodeCsvReaderActor = Context.ActorOf(Props.Create<CsvReaderActor<PostcodeRow, PostcodeMap>>(postcodeSourcePath), "PostcodeCsvReaderActor");
            _postcodeReferenceDataWriter = new PostcodeReferenceDataWriter();
            _locationReferenceDataWriter = new LocationReferenceDataWriter();
            _logger = Context.GetLogger();

            Become(ProcessPostcodeData);
        }

        private void CreateAddressReferenceDataReaderActor()
        {
            var sourcePath = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"],
                "AddressReferenceData.csv");
            _addressReferenceDataReaderActor = Context.ActorOf(Props
                .Create<CsvReaderActor<LocationReferenceDataRow, LocationReferenceDataMap>>(sourcePath), "AddressReferenceDataCsvReaderActor");
        }

        private void ProcessPostcodeData()
        {
            Receive<InitiateAnalysisMessage>(message =>
            {
                _logger.Info($"Received {nameof(message)}, proceeding with Postcode Reference Data generation.");
                _postcodeCsvReaderActor.Tell(new InitiateAnalysisMessage());
            });

            Receive<DataRowMessage>(message => PopulateReferenceData(message));

            Receive<FileAnalysisFinishedMessage>(message =>
            {
                WriteReferenceData();
                Become(ProcessAddressReferenceData);
                CreateAddressReferenceDataReaderActor();
                _addressReferenceDataReaderActor.Tell(new InitiateAnalysisMessage());
            });
        }

        private void ProcessAddressReferenceData()
        {
            Receive<InitiateAnalysisMessage>(message =>
            {
                _logger.Info($"Received {nameof(message)}, proceeding with Location Reference data generation.");
            });
            Receive<DataRowMessage>(message => PopulateLocationReferenceData(message));
            Receive<FileAnalysisFinishedMessage>(message => WriteLocationReferenceData());
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
        }

        private void PopulateLocationReferenceData(DataRowMessage message)
        {
            _locationReferenceDataWriter.UpdateReferenceData(message.Row);
        }

        private void WriteLocationReferenceData()
        {
            _logger.Info("Received FileAnalysisFinishedMessage, commiting changes");
            _locationReferenceDataWriter.WriteReferenceData();
            _logger.Info("Finished writing Location Reference Data.");
            Context.Parent.Tell(new FileAnalysisFinishedMessage());
        }
    }
}