using CsvHelper;

namespace NHSData.Common
{
    public interface IConfiguration
    {
        ICsvReader Reader { get; }
    }
}