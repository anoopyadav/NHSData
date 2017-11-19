using CsvHelper.Configuration;
using NHSData.DataObjects;

namespace NHSData.CsvMaps
{
    public sealed class PrescriptionMap :  CsvClassMap<PrescriptionRow>
    {
        public PrescriptionMap()
        {
            Map(m => m.Sha).Name("SHA");
            Map(m => m.Pct).Name("PCT");
            Map(m => m.PracticeCode).Name("PracticeCode");
            Map(m => m.PrescriptionCode).Name("BNF CODE");
            Map(m => m.PrescriptionName).Name("BNF NAME");
            Map(m => m.NumberOfItems).Name("ITEMS");
            Map(m => m.IndicatedCost).Name("NIC");
            Map(m => m.ActualCost).Name("ACT COST");
            Map(m => m.Period).Name("PERIOD");
        }
    }
}