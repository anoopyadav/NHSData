using Akka.Actor;
using CsvHelper;
using NHSData.Common;
using NHSData.Messages;
using NHSData.ReferenceData;

namespace NHSData.Actors
{
    public class ReferenceDataCreatorActor : ReceiveActor
    {
        private readonly IReferenceDataReader _dataReader;
        private readonly IReferenceDataWriter _dataWriter;
        public ReferenceDataCreatorActor(IConfiguration configuration)
        {
            _dataReader = configuration.ReferenceDataReader;
            _dataWriter = configuration.ReferenceDataWriter;

            Receive<InitiateAnalysisMessage>(message => PopulateReferenceData());
        }

        private void PopulateReferenceData()
        {
            _dataReader.LoadReferenceData();
            _dataWriter.WriteReferenceData();

            Context.Parent.Tell(new ReferenceDataAnalysisFinishedMessage());
        }
    }
}