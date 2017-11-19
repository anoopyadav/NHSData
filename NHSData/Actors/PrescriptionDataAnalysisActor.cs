using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Akka.Actor;
using NHSData.CsvMaps;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;
using NHSData.ReferenceData;

namespace NHSData.Actors
{
    public class PrescriptionDataAnalysisActor<TRowType, TRowMap> : BaseDataAnalysisActor<TRowType, TRowMap>
    {
        private IActorRef _prescriptionCsvReaderActor;
        private Dictionary<string, string> _practiceCodeToRegion;
        private Dictionary<string, string> _practicecodeToPostcode;
        private IActorRef _regionReferenceDataReaderActor;
        private IActorRef _postcodeReferenceDataReaderActor;
        private int _referenceDataCount;

        public PrescriptionDataAnalysisActor(IDataAnalyzer prescriptionAnalyzer, string sourcePath) 
            : base(prescriptionAnalyzer, sourcePath)
        {
            _practiceCodeToRegion = new Dictionary<string, string>();
            _practicecodeToPostcode = new Dictionary<string, string>();

            Receive<LoadReferenceDataMessage>(message => Become(LoadReferenceData));
        }

        private void LoadReferenceData()
        {
            Logger.Info("Received InitiateAnalysisMessage, loading location reference data");
            CreateReferenceDataReaderActors();
            _regionReferenceDataReaderActor.Tell(new InitiateAnalysisMessage());
            _postcodeReferenceDataReaderActor.Tell(new InitiateAnalysisMessage());

            Receive<DataRowMessage>(message => PopulateReferenceData(message));
            Receive<FileAnalysisFinishedMessage>(message => CheckReferenceDataLoaded());
        }

        private void CheckReferenceDataLoaded()
        {
            _referenceDataCount++;

            if (_referenceDataCount == 2)
            {
                Logger.Info("Received FileAnalysisFinishedMessage, proceeding with prescription analysis");
                Become(ProcessPrescriptionData);
            }
        }

        private void ProcessPrescriptionData()
        {
            Context.Parent.Tell(new FileAnalysisFinishedMessage());
        }

        private void PopulateReferenceData(DataRowMessage message)
        {
            if (message.RowType == typeof(PostcodeReferenceDataRow))
            {
                var postcodeRow = (PostcodeReferenceDataRow) message.Row;
                _practicecodeToPostcode.Add(postcodeRow.Postcode, postcodeRow.Region);

            }
            else if (message.RowType == typeof(LocationReferenceDataRow))
            {
                var locationRow = (LocationReferenceDataRow) message.Row;
                _practiceCodeToRegion.Add(locationRow.PracticeCode, locationRow.Postcode);
            }
        }

        private void CreateReferenceDataReaderActors()
        {
            var sourcePath = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "RegionReferenceData.csv");
            _regionReferenceDataReaderActor =
                Context.ActorOf(Props.Create <CsvReaderActor<PostcodeReferenceDataRow, PostcodeReferenceDataMap>>(sourcePath));

            sourcePath = Path.Combine(ConfigurationManager.AppSettings["DataDirectory"], "AddressReferenceData.csv");
            _postcodeReferenceDataReaderActor =
                Context.ActorOf(
                    Props.Create<CsvReaderActor<LocationReferenceDataRow, LocationReferenceDataMap>>(sourcePath));
        }

        protected override void ProcessRow(DataRowMessage message)
        {
            throw new System.NotImplementedException();
        }

        protected override void PostAnalysis()
        {
            throw new System.NotImplementedException();
        }
    }
}