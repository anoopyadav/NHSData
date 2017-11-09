using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using CsvHelper;
using NHSData.Common;
using NHSData.DataObjects;
using NHSData.Messages;
using NHSData.ReferenceData;
using NLog;

namespace NHSData.Actors
{
    public class ReferenceDataCreatorActor : ReceiveActor
    {
        private readonly ICsvReader _postcodeCsvReader; 
        private readonly IReferenceDataReader _addressReferenceDataReader;
        private readonly IReferenceDataWriter _postcodeReferenceDataWriter;
        private ILoggingAdapter Logger;
        public ReferenceDataCreatorActor(IConfiguration configuration)
        {
            _addressReferenceDataReader = configuration.ReferenceDataReader;
            _postcodeReferenceDataWriter = configuration.ReferenceDataWriter;
            _postcodeCsvReader = configuration.Reader;
            Logger = Context.GetLogger();

            Receive<InitiateAnalysisMessage>(message =>
            {
                Logger.Info($"Received {nameof(message)}, proceeding with Reference Data generation.");
                PopulateReferenceData();
            });
        }

        private void PopulateReferenceData()
        {
            // Step 1: Load Address Reference Data
            //_addressReferenceDataReader.LoadReferenceData();
            //var practiceToPostcodeLookup = _addressReferenceDataReader.GetReferenceData();

            // Step 3: Generate Practice to Region lookup
            //_dataWriter.WriteReferenceData();
            while (_postcodeCsvReader.Read())
            {
                var row = _postcodeCsvReader.GetRecord<PostcodeRow>();
                _postcodeReferenceDataWriter.UpdateReferenceData(row);
            }

            Logger.Info($"Analysis Complete, writing to file.");
            _postcodeReferenceDataWriter.WriteReferenceData();

            Context.Parent.Tell(new FileAnalysisFinishedMessage());
        }
    }
}