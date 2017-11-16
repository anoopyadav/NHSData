using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.CsvMaps
{
    public sealed class AddressReferenceDataMap : CsvClassMap<AddressReferenceDataRow>
    {
        public AddressReferenceDataMap()
        {
            Map(m => m.PracticeCode).Name("PracticeCode");
            Map(m => m.Postcode).Name("Postcode");
        }
    }

}