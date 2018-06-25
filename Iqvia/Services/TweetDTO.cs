using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Iqvia.Services
{
    public class TweetDTO
    {
        public DateTime stamp { get; set; }
        public string text { get; set; }
        public string id { get; set; }

        public override bool Equals(object x)
        {
            return ((TweetDTO)x).id == id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}