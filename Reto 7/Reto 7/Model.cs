using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto7
{
    public class Images
    {
        public string poster { get; set; }
        public string fanart { get; set; }
        public string banner { get; set; }
    }

    public class Ratings
    {
        public int percentage { get; set; }
        public int votes { get; set; }
        public int loved { get; set; }
        public int hated { get; set; }
    }

    public class Film
    {
        public string title { get; set; }
        public int year { get; set; }
        public string url { get; set; }
        public int first_aired { get; set; }
        public string country { get; set; }
        public string overview { get; set; }
        public int runtime { get; set; }
        public string status { get; set; }
        public string network { get; set; }
        public string air_day { get; set; }
        public string air_time { get; set; }
        public string certification { get; set; }
        public string imdb_id { get; set; }
        public string tvdb_id { get; set; }
        public string tvrage_id { get; set; }
        public string poster { get; set; }
        public Images images { get; set; }
        public int watchers { get; set; }
        public Ratings ratings { get; set; }
        public List<string> genres { get; set; }
    }
}
