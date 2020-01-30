using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TextFileGenerator.ValueCalculators;

namespace TextFileGenerator
{
    class Program
    {
        public static string secretsPath = @"Secrets.txt";
        
        //public static string _resultPath = @"D:\StreamStuff\TextFileGenerator\Results";
        //public static string _p12Path = @"D:\StreamStuff\TextFileGenerator\TextFileGenerator\textgeneratorfortwitch-fb45dcb9f941.p12";

        //public static string _user = "admin-613@textgeneratorfortwitch.iam.gserviceaccount.com";
        //public static string _spreadsheetId = "1zxZkLuLzSRmnni3vuRgWiFXMasR-CQSKe0_6f5Fxn9w";
        //public static int _refreshRate = 300000 / 5;

        public static Dictionary<string, string> secretDictionary = new Dictionary<string, string>();


        public static string _sheetName = null;

        static void Main(string[] args)
        {

            while (false)
            {
                LoadSecretss();
                var client = new SheetsApiClient();
                RefreshSheetName(client);

                List<IValueCalculator> valueCalculators = new List<IValueCalculator>();
                valueCalculators.Add(new WLRecordCalc(client));
                valueCalculators.Add(new TopScorerCalc(client));


                Directory.CreateDirectory(secretDictionary["ResultPath"]);

                foreach (var calculation in valueCalculators)
                {
                    CreateFile(calculation);
                }
                Thread.Sleep(int.Parse(secretDictionary["RefreshRate"])); // 5 minutes = 300000
            }

            while (true) {

                var client = new FifaApiClient();

                var a = client.getFifaApi(new DateTime(2020-01-29), new DateTime(2020-01-30), Guid.Parse("e040fa72-4a4c-495f-8a5f-8c826fd7756a"));
                var b = client.getFifaApi(new DateTime(01-29-2020), new DateTime(01-30-2020), Guid.Parse("e040fa72-4a4c-495f-8a5f-8c826fd7756a"));
                Console.WriteLine(a);
                

            }
        }

        private static void LoadSecretss()
        {
            using (var sr = new StreamReader(secretsPath))
            {
                secretDictionary.Clear();
                while (!sr.EndOfStream)
                {
                    var secret = sr.ReadLine();
                    var keyValue = secret.Split('=');
                    secretDictionary.Add(keyValue[0], keyValue[1]);
                } 
            }
        }

        public static void CreateFile(IValueCalculator calculator)
        {
                using (StreamWriter streamWriter = new StreamWriter(Path.Combine(secretDictionary["ResultPath"], calculator.GetFileName())))
                {
                var result = calculator.GetResult();
                    streamWriter.WriteLine(result);
                    Console.WriteLine($"{DateTime.Now}: {calculator.GetFileName()} was updated to: '{result}'");
                }
        }

        public static DateTime StartOfWeek(DateTime date)
        {
            date = date.Date; // Just remove any time parts...
                              // 0=Sunday, 1=Monday... 6=Saturday
            int sundayToSaturdayDayOfWeek = (int)date.DayOfWeek;
            // 0=Monday, 1=Tuesday... 6=Sunday
            int mondayToSundayDayOfWeek = ((sundayToSaturdayDayOfWeek - 1) + 7) % 7;
            return date.AddDays(-mondayToSundayDayOfWeek);
        }

        public static void RefreshSheetName(SheetsApiClient sheetsApiClient)
        {
           string sheetPrefix = "WL - ";

            var mondayInCurrentWeek = StartOfWeek(DateTime.Today);

            var fridayThisWeek = mondayInCurrentWeek.AddDays(4).ToString().Split(' ');
            var fridayLastWeek = mondayInCurrentWeek.AddDays(-3).ToString().Split(' ');

            try
            {
                var sheet = $"{sheetPrefix}{fridayThisWeek[0]}";
                var result = sheetsApiClient.GetValueFromSheet("B5", sheet);
                _sheetName = sheet;
                return;
            } catch(Exception e)
            {

            }

            try
            {
                var sheet = $"{sheetPrefix}{fridayLastWeek[0]}";
                var result = sheetsApiClient.GetValueFromSheet("B5", sheet);
                _sheetName = sheet;
                return;
            } catch(Exception e)
            {

            }
           


        }
    }
}
