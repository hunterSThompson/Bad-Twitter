using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Iqvia.Services
{
    /// <summary>
    /// Wrapper class for the Twitter REST API.
    /// </summary>
    class TwitterApiClient
    {
        //string apiBaseUrl = ConfigurationManager.AppSettings["api.baseurl"];
        string apiBaseUrl = "https://badapi.iqvia.io";

        public async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiBaseUrl);

                string startStr = ConvertToUtcString(start);
                string endStr = ConvertToUtcString(end);

                var result = await client.GetStringAsync($"/api/v1/Tweets?startDate={startStr}&endDate={endStr}");
                var tweets = JsonConvert.DeserializeObject<List<TweetDTO>>(result);

                return tweets;
            }
        }

        private string ConvertToUtcString(DateTime datetime)
        {
            return datetime.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'");
        }
    }
}