using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileGenerator.ValueCalculators
{
    public class WLRecordCalc : IValueCalculator
    {
        private SheetsApiClient _sheetsApiClient;

        private string _coordinates = "B5:B6";

        private Task<string> _task;

        public WLRecordCalc(SheetsApiClient sheetsApiClient)
        {
            _sheetsApiClient = sheetsApiClient;

            _task = new Task<string>(() =>
            {
                var mondayInCurrentWeek = Program.StartOfWeek(DateTime.Today);

                try
                {
                    return GetWLScore();
                }
                catch (Exception e)
                {
                }

                return "N/A"; ;

            });
            _task.Start();
        }
        
        public string GetFileName()
        {
            return "WLRecord.txt";
        }

        public string GetResult()
        {
            var toReturn = _task.Result;
            return toReturn;
        }

        public string GetWLScore()
        {
            var sheet = $"{Program._sheetName}";
            var result = _sheetsApiClient.GetValueFromSheet(_coordinates, sheet);
            var wins = int.Parse((string)result.Values[0][0]);
            var losses = int.Parse((string)result.Values[1][0]);
            
            return $"{wins} - {losses}";
        }
    }
}
