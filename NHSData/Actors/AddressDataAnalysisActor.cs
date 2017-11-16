using System;
using NHSData.DataAnalyzers;
using NHSData.Messages;
using NHSData.ReferenceData;

namespace NHSData.Actors
{
    public class AddressDataAnalysisActor<TRowType, TRowMap> : BaseDataAnalysisActor<TRowType, TRowMap>
    {
        private readonly IReferenceDataWriter _referenceDataWriter;

        public AddressDataAnalysisActor(IDataAnalyzer analyzer, string sourcePath)
            : base(analyzer, sourcePath)
        {
            _referenceDataWriter = new AddressReferenceDataWriter();
        }

        protected override void ProcessRow(DataRowMessage message)
        {
            dynamic row = Convert.ChangeType(message.Row, message.RowType);
            Analyzer.ConsumeRow(row);
            _referenceDataWriter.UpdateReferenceData(row);
        }

        protected override void PostAnalysis()
        {
            Logger.Info("Publishing Address Reference Data.");
            _referenceDataWriter.WriteReferenceData();
            Context.Parent.Tell(new FileAnalysisFinishedMessage(), Self);
        }
    }
}