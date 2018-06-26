using Iqvia.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Iqvia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<string> GetData(DateTime start, DateTime end)
        {
            var tweets = await TweetService.GetTweets(start, end, 4);
            return JsonConvert.SerializeObject(tweets);
        }
    }
}