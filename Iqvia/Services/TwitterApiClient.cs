using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Iqvia.Services
{
    class TwitterApiClient
    {
        int numThreads = 2;
        string apiBaseUrl = "https://badapi.iqvia.io";

        public async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                string startStr = ConvertToUtcString(start);
                string endStr = ConvertToUtcString(end);

                var result = await client.GetStringAsync($"/api/v1/Tweets?startDate={startStr}&endDate={endStr}");
                var tweets = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TweetDTO>>(result);

                return tweets;
            }
        }

        private string ConvertToUtcString(DateTime datetime)
        {
            return datetime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}