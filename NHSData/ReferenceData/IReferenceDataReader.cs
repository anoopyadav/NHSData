using System.Collections;
using System.Collections.Generic;

namespace NHSData.ReferenceData
{
    public interface IReferenceDataReader
    {
        void LoadReferenceData();
        IDictionary<string, string> GetReferenceData();
    }
}