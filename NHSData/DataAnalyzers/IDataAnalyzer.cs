using System;
using System.Collections;
using System.Collections.Generic;
using NHSData.DataObjects;
using NLog.LogReceiverService;

namespace NHSData.DataAnalyzers
{
    public interface IDataAnalyzer
    {
        void ConsumeRow(IDataRow row);
        IEnumerable<Tuple<string, string>> GetResults();
        void PublishResults();
    }
}