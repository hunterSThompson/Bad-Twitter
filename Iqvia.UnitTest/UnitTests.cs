using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Iqvia.Services;

namespace Iqvia.UnitTest
{
    [TestClass]
    public class TweetServiceTests
    {
        [TestMethod]
        public void VerifyConcurrentMethod()
        {
            var tweetsFromSyncCall1 = TweetService.GetTweets(new DateTime(2017, 1, 1), new DateTime(2017, 12, 31)).Result;
            var tweetsAsync1 = TweetService.GetTweets(new DateTime(2017, 1, 1), new DateTime(2017, 12, 31), 4).Result;

            var tweetsFromSyncCall2 = TweetService.GetTweets(new DateTime(2017, 1, 1), new DateTime(2017, 6, 6)).Result;
            var tweetsAsync2 = TweetService.GetTweets(new DateTime(2017, 1, 1), new DateTime(2017, 6, 6), 4).Result;

            var tweetsFromSyncCall3 = TweetService.GetTweets(new DateTime(2017, 2, 1), new DateTime(2017, 3, 15)).Result;
            var tweetsAsync3 = TweetService.GetTweets(new DateTime(2017, 2, 1), new DateTime(2017, 3, 15), 4).Result;

            // Insure the concurrent method and sync method returns the same thing.
            Assert.AreEqual(tweetsAsync1.Count, tweetsFromSyncCall1.Count);
            Assert.AreEqual(tweetsAsync1.Count, tweetsFromSyncCall1.Count);
            Assert.AreEqual(tweetsAsync1.Count, tweetsFromSyncCall1.Count);
        }

        [TestMethod]
        public void VerifyUniqueness()
        {
            var tweetsAsync = TweetService.GetTweets(new DateTime(2017, 1, 1), new DateTime(2017, 12, 31), 4).Result;

            // Verify that we don't have any duplicate tweets
            Assert.IsTrue(tweetsAsync.Distinct().Count() == tweetsAsync.Count);
        }
    }
}
