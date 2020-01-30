using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileGenerator.ValueCalculators
{
    public class TopScorerCalc : IValueCalculator
    {

        private SheetsApiClient _sheetsApiClient;

        private string _coordinates = "K27:X58";
        

        private Task<string> _task;

        public TopScorerCalc(SheetsApiClient sheetsApiClient)
        {
            _sheetsApiClient = sheetsApiClient;

            _task = new Task<string>(() =>
            {
                try
                {
                    return GetWLTopScorer();
                } catch(Exception e) { }
                
                return "N/A"; ;

            });
            _task.Start();
        }

        public string GetWLTopScorer()
        {

            var sheet = $"{Program._sheetName}";
            var result = _sheetsApiClient.GetValueFromSheet(_coordinates, sheet);

            var IndexMax = 0;
            var numberOfGoalsMax = 0;

            var playerNames = result.Values[0];
            var numberOfGoals = result.Values[31];

            for(int i = 0;i<numberOfGoals.Count;i++)
            {
                try
                {
                    var goals = int.Parse((string)numberOfGoals[i]);

                    if(goals > numberOfGoalsMax)
                    {
                        numberOfGoalsMax = goals;
                        IndexMax = i;

                    }
                }catch(Exception e)
                {

                }

            }

            if (numberOfGoalsMax == 0)
                throw new Exception();

            var topScorer = (string) playerNames[IndexMax];

            return $"{topScorer} ({numberOfGoalsMax} basser)";
        }

        public string GetFileName()
        {
            return "Topscorer.txt";
        }

        public string GetResult()
        {
            return _task.Result;
        }
    }
}
