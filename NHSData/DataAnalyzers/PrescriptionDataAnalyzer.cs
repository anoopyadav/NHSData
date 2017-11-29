using System;
using System.Collections.Generic;
using System.Linq;
using NHSData.DataObjects;

namespace NHSData.DataAnalyzers
{
    public class PrescriptionDataAnalyzer : IDataAnalyzer
    {
        private double _totalCost;
        private int _totalCostCount;
        private Dictionary<string, double> _postcodesByActualSpend = new Dictionary<string, double>();
        public Dictionary<string, string> PracticeCodeToPostcode { get; set;}
        public Dictionary<string, string> PostcodeToRegion { get; set; }
        public void ConsumeRow(IDataRow row)
        {
            var prescriptionRow = row as PrescriptionRow;

            if (prescriptionRow == null)
            {
                return;
            }

            // Track cost of Peppermint Oil
            if (prescriptionRow.PrescriptionName.Trim().Equals("Peppermint Oil"))
            {
                _totalCost += prescriptionRow.ActualCost;
                _totalCostCount++;
            }

            // Track Total Cost across Postcodes
            string postcode = "UNKNOWN";
            if (PracticeCodeToPostcode.ContainsKey(prescriptionRow.PracticeCode))
            {
                postcode = PracticeCodeToPostcode[prescriptionRow.PracticeCode];
            }
            if (!_postcodesByActualSpend.ContainsKey(postcode))
                _postcodesByActualSpend[postcode] = 0.0;
            _postcodesByActualSpend[postcode] += prescriptionRow.ActualCost;
        }

        private double CalculateAverageCostOfPrescription()
        {
            return _totalCost / _totalCostCount;
        }

        private IEnumerable<string> GetTop5Spenders()
        {
            var topSpenders = _postcodesByActualSpend.OrderByDescending(x => x.Value).Take(5).Select(x => x.Key);
            return topSpenders.ToList();
        }

        public IEnumerable<Tuple<string, string>> GetResults()
        {
            var results = new List<Tuple<string, string>>();
            results.Add(new Tuple<string, string>("Average cost of Peppermint Oil: ", CalculateAverageCostOfPrescription().ToString()));
            results.Add(new Tuple<string, string>("Top 5 spenders: ", String.Join(",", GetTop5Spenders())));
            return results;
        }

        public void PublishResults()
        {
            
        }
    }
}