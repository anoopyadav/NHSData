using System;
using System.ComponentModel;
using NHSData.Common;
using NHSData.DataAnalyzers;
using NHSData.DataObjects;
using NHSData.Messages;

namespace NHSData.Actors
{
    public class AddressDataAnalysisActor : BaseDataAnalysisActor
    {
        public AddressDataAnalysisActor(IDataAnalyzer analyzer, IConfiguration configuration)
            : base(analyzer, configuration)
        {

        }

        protected override void PerformAnalysis()
        {
            while (CsvReader.Read())
            {
                var row = CsvReader.GetRecord<Address>();
                Analyzer.ConsumeRow(row);
            }
            Context.Parent.Tell(new FileAnalysisFinishedMessage(), Self);
        }
    }
}