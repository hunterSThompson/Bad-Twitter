using Iqvia.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Iqvia.Controllers
{
    public class HomeController : Controller
    {
        int _concurrencyLevel = int.Parse(ConfigurationManager.AppSettings["Concurrency.Level"]);

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> GetData(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new Exception("start date can't be greater than end!");
            }

            var tweets = await TweetService.GetTweets(start, end, _concurrencyLevel);
            return JsonConvert.SerializeObject(tweets);
        }
    }
}