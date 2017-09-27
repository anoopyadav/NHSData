namespace NHSData.DataObjects
{
    public class AddressRow : IDataRow
    {
        public string Period { get; set; }
        public string PracticeCode { get; set; }
        public string PracticeName { get; set; }
        public string PracticeBuilding { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Town { get; set; }
        public string Postcode { get; set; }
    }
}
