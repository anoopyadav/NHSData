using System;
using System.Collections.Generic;
using NHSData.DataObjects;

namespace NHSData.DataAnalyzers
{
    public class PrescriptionDataAnalyzer : IDataAnalyzer
    {
        public void ConsumeRow(IDataRow row)
        {
            
        }

        public IEnumerable<Tuple<string, string>> GetResults()
        {
            return null;
        }

        public void PublishResults()
        {
            
        }
    }
}