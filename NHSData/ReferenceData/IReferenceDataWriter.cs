using NHSData.DataObjects;

namespace NHSData.ReferenceData
{
    public interface IReferenceDataWriter
    {
        void UpdateReferenceData(IDataRow row);
        void WriteReferenceData();
    }
}