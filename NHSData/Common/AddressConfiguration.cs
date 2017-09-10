using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.CsvMaps;
using NHSData.DataObjects;

namespace NHSData.Common
{
    public class AddressConfiguration : IConfiguration
    {
        public ICsvReader Reader { get; }

        public AddressConfiguration()
        {
            var csvConfiguration = new CsvConfiguration();
            csvConfiguration.RegisterClassMap<AddressMap>();
            Reader = new CsvReader(new StreamReader(File.OpenRead("C:\\Users\\sir_snoop_a_lot\\Downloads\\address.csv")), csvConfiguration);
        }
    }
}