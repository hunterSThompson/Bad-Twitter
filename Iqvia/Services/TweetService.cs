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
        public static async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end, int concurrencyLevel)
        {
            // Calculate the chunk size
            int totalDays = (end - start).Days;
            int daysPerChunk = totalDays / concurrencyLevel;

            // Create a list of tasks
            var tasks = new List<Task<List<TweetDTO>>>();

            // Iterate through each chunk of the range and kick off a task to fetch those tweets.
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(daysPerChunk)) < end)
            {
                var fetchTask = GetTweets(start, chunkEnd);
                tasks.Add(fetchTask);
                start = chunkEnd;
            }

            var result = await Task.WhenAll(tasks);

            var all = new List<TweetDTO>();
            foreach (var res in result)
            {
                all.AddRange(res);
            }

            var set = new HashSet<string>();

            foreach (var thing in all)
            {
                if (!set.Contains(thing.id)) set.Add(thing.id);
                else
                {

                }
            }

            return all;
        }

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