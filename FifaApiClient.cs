using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextFileGenerator {
    public class FifaApiClient {

        private RestClient client;
        public FifaApiClient() {
            client = new RestClient("http://fifabotapi.azurewebsites.net");
        }
        public string getFifaApi(DateTime startDate, DateTime endDate, Guid userId) {
            var request = new RestRequest("GetTopscorerForPeriod", Method.GET);

            request.AddQueryParameter("start", startDate.ToString());
            request.AddQueryParameter("end", endDate.ToString());
            request.AddQueryParameter("userId", userId.ToString());
            var response = client.Execute(request);

            return response.Content;

        }
    }
}
