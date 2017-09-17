using System.Configuration;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using NHSData.CsvMaps;
using NHSData.ReferenceData;

namespace NHSData.Common
{
    public class AddressConfiguration : IConfiguration
    {
        public ICsvReader Reader { get; }
        public IReferenceDataWriter ReferenceDataWriter { get; }

        public AddressConfiguration()
        {
            var csvConfiguration = new CsvConfiguration();
            csvConfiguration.RegisterClassMap<AddressMap>();
            var path = Path.Combine
                (ConfigurationManager.AppSettings["DataDirectory"], "address.csv");
            Reader = new CsvReader(new StreamReader(File.OpenRead(path)), csvConfiguration);
            ReferenceDataWriter = new AddressReferenceDataWriter();
        }
    }
}