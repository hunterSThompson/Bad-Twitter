using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Iqvia.Services
{
    class TweetService
    {
        static public async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end)
        {
            var client = new TwitterApiClient();

            var tweets = await client.GetTweets(start, end);
            var set = new HashSet<TweetDTO>(tweets);

            while (tweets.Count > 0)
            {
                start = tweets[tweets.Count - 1].stamp;

                //Console.WriteLine($"Fetching from {start} to {end}");

                tweets = await client.GetTweets(start, end);

                bool foundNew = false;
                foreach (var tweet in tweets)
                {
                    if (!set.Contains(tweet))
                    {
                        foundNew = true;
                        set.Add(tweet);
                    }
                };

                if (!foundNew)
                {
                    break;
                }
            }

            return set.ToList();
        }
    }
}