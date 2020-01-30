using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TextFileGenerator
{
    public class SheetsApiClient
    {
        public static IEnumerable<string> Scopes { get; private set; }

        public Google.Apis.Sheets.v4.Data.ValueRange GetValueFromSheet(string coordinate, string sheetName)
        {
            SheetsService sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google-SheetsSample/0.1",
            });

            // The ID of the spreadsheet to retrieve data from.
            string spreadsheetId = Program.secretDictionary["SpreadsheetId"];  // TODO: Update placeholder value.

            // The A1 notation of the values to retrieve.
            string range = $"{sheetName}!{coordinate}";  // TODO: Update placeholder value.

            // How values should be represented in the output.
            // The default render option is ValueRenderOption.FORMATTED_VALUE.
            SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum valueRenderOption = (SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum)0;  // TODO: Update placeholder value.

            // How dates, times, and durations should be represented in the output.
            // This is ignored if value_render_option is
            // FORMATTED_VALUE.
            // The default dateTime render option is [DateTimeRenderOption.SERIAL_NUMBER].
            SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum dateTimeRenderOption = (SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum)0;  // TODO: Update placeholder value.

            SpreadsheetsResource.ValuesResource.GetRequest request = sheetsService.Spreadsheets.Values.Get(spreadsheetId, range);
            request.ValueRenderOption = valueRenderOption;
            request.DateTimeRenderOption = dateTimeRenderOption;

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            // Data.ValueRange response = await request.ExecuteAsync();

            return response;
        }

        public ServiceAccountCredential GetCredential()
        {
            // TODO: Change placeholder below to generate authentication credentials. See:
            // https://developers.google.com/sheets/quickstart/dotnet#step_3_set_up_the_sample
            //
            // Authorize using one of the following scopes:
            //     "https://www.googleapis.com/auth/drive"
            //     "https://www.googleapis.com/auth/drive.file"
            //     "https://www.googleapis.com/auth/drive.readonly"
            //     "https://www.googleapis.com/auth/spreadsheets"
            //     "https://www.googleapis.com/auth/spreadsheets.readonly"

            var certificate = new X509Certificate2(Program.secretDictionary["P12Path"], "notasecret", X509KeyStorageFlags.Exportable);

            string user = Program.secretDictionary["User"];

            var serviceAccountCredentialInitializer = new ServiceAccountCredential.Initializer(user)
            {
                Scopes = new[] { "https://spreadsheets.google.com/feeds" }
            }.FromCertificate(certificate);

            var credential = new ServiceAccountCredential(serviceAccountCredentialInitializer);

            return credential;

        }
    }

    
}
