using CsvHelper;
using NHSData.ReferenceData;

namespace NHSData.Common
{
    public interface IConfiguration
    {
        ICsvReader Reader { get; }
        IReferenceDataWriter ReferenceDataWriter { get; }
    }
}