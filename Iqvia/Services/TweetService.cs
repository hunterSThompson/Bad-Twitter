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
        /// <summary>
        /// Fetch tweets in between the supplied dates, using parallel processing to speed up 
        /// the method.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="concurrencyLevel"></param>
        /// <returns></returns>
        public static async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end, int concurrencyLevel)
        {
            // Calculate the chunk size
            int totalDays = (end - start).Days;
            int daysPerChunk = totalDays / concurrencyLevel;

            // Create a list of tasks
            //var tasks = new List<Task<List<TweetDTO>>>();
            var tasks = new List<Task<List<TweetDTO>>>();

            // Iterate thorugh each date range and fetch tweets
            var chunks = SplitDateRange(start, end, daysPerChunk);
            foreach (var chunk in chunks)
            {
                var fetchTask = GetTweets(chunk.Item1, chunk.Item2);
                tasks.Add(fetchTask);
            }

            // Wait until all requests finish
            var result = await Task.WhenAll(tasks);

            // Combine all results into a single list
            return result.SelectMany(x => x).ToList();
        }

        /// <summary>
        /// A method to divide a date range into enumerable collection of tuples containing the 
        /// start date and end date of each chunk.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="dayChunkSize"></param>
        /// <returns></returns>
        private static IEnumerable<Tuple<DateTime, DateTime>> SplitDateRange(DateTime start, DateTime end, int dayChunkSize)
        {
            DateTime chunkEnd;
            while ((chunkEnd = start.AddDays(dayChunkSize)) < end)
            {
                yield return Tuple.Create(start, chunkEnd);
                start = chunkEnd;
            }
            yield return Tuple.Create(start, end);
        }

        /// <summary>
        /// Fetch tweets in between date range.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        static public async Task<List<TweetDTO>> GetTweets(DateTime start, DateTime end)
        {
            var client = new TwitterApiClient();

            // Fetch first batch of tweets and save in set.
            var tweets = await client.GetTweets(start, end);
            var set = new HashSet<TweetDTO>(tweets);

            // Fetch tweets until the API stops returning new tweets.
            while (tweets.Count > 0)
            {
                // Use the prior last tweet's timestamp as the start time for the next batch.
                start = tweets[tweets.Count - 1].stamp;
                tweets = await client.GetTweets(start, end);

                // Add each new tweet to the set
                bool foundNew = false;
                foreach (var tweet in tweets)
                {
                    if (!set.Contains(tweet))
                    {
                        foundNew = true;
                        set.Add(tweet);
                    }
                };

                // If we didn't find any new tweets, we're done.
                if (!foundNew)
                {
                    break;
                }
            }

            return set.ToList();
        }
    }
}