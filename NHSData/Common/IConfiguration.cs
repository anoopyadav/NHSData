using CsvHelper;
using NHSData.ReferenceData;

namespace NHSData.Common
{
    public interface IConfiguration
    {
        ICsvReader Reader { get; }
        IReferenceDataReader ReferenceDataReader { get; }
        IReferenceDataWriter ReferenceDataWriter { get; }
    }
}