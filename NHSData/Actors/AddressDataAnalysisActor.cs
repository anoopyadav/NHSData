using System;
using System.Collections.Generic;
using System.ComponentModel;
using CsvHelper;
using NHSData.Common;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;
using NHSData.ReferenceData;

namespace NHSData.Actors
{
    public class AddressDataAnalysisActor : BaseDataAnalysisActor
    {
        private readonly IReferenceDataWriter _referenceDataWriter;
        private readonly IDictionary<string, string> _practicesToPostcodeMap;

        public AddressDataAnalysisActor(IDataAnalyzer analyzer, IConfiguration configuration)
            : base(analyzer, configuration)
        {
            _referenceDataWriter = configuration.ReferenceDataWriter;
            _practicesToPostcodeMap = new Dictionary<string, string>();
        }

        protected override void PerformAnalysis()
        {
            while (CsvReader.Read())
            {
                var row = CsvReader.GetRecord<Address>();
                Analyzer.ConsumeRow(row);
                _referenceDataWriter.UpdateReferenceData(row);
            }
            Logger.Info("Publishing Address Reference Data.");
            _referenceDataWriter.WriteReferenceData();
            Context.Parent.Tell(new FileAnalysisFinishedMessage(), Self);
        }
    }
}