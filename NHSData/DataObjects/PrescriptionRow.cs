namespace NHSData.DataObjects
{
    public class PrescriptionRow : IDataRow
    {
        public string Sha { get; set; }
        public string Pct { get; set; }
        public string PracticeCode { get; set; }
        public string PrescriptionCode { get; set; }
        public string PrescriptionName { get; set; }
        public int NumberOfItems { get; set; }
        public double IndicatedCost { get; set; }
        public double ActualCost { get; set; }
        public string Period { get; set; }
    }
}