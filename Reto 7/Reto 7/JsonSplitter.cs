using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reto7
{
    public class JsonSplitter
    {

        /*
         * Método mas rápido usando directamente la clase JArray de la librería http://james.newtonking.com/json
         */ 
        public static Tuple<string, string> SplitShowsByGenre(string trendingShowsJson, string p)
        {
            JArray a = new JArray(); JArray b = new JArray();
            JArray list = JArray.Parse(trendingShowsJson);

            list.ToList().ForEach(v => {
                if (v["genres"].Values<string>().Contains(p))
                    a.Add(v);
                else
                    b.Add(v);
            });

            return new Tuple<string, string>(
                a.ToString(Formatting.None),
                b.ToString(Formatting.None)
            );
        }

        /*
         * Método mas compacto para procesar JSON pero mas lento
         * ademas necesita las clases tipadas que he generado con http://json2csharp.com/
         */
        public static Tuple<string, string> SplitShowsByGenre2(string trendingShowsJson, string p)
        {
            var list = JsonConvert.DeserializeObject<Film[]>(trendingShowsJson);

            return new Tuple<string, string>(
                JsonConvert.SerializeObject(list.Where(f => f.genres.Contains(p))),
                JsonConvert.SerializeObject(list.Where(f => !f.genres.Contains(p)))
            );
        }


        #region Otras variantes
        public static Tuple<string, string> SplitShowsByGenre3(string trendingShowsJson, string p)
        {
            JArray list = JArray.Parse(trendingShowsJson);

            JArray a = new JArray();
            JArray b = new JArray();
            list.Where(f => f["genres"].Values<string>().Contains(p)).ToList().ForEach(v =>  a.Add(v));
            list.Where(f => !f["genres"].Values<string>().Contains(p)).ToList().ForEach(v => b.Add(v));

            return new Tuple<string, string>(
                a.ToString(Formatting.None),
                b.ToString(Formatting.None)
            );
        }

        public static Tuple<string, string> SplitShowsByGenre4(string trendingShowsJson, string p)
        {
            JsonTextReader tr = new JsonTextReader(new StringReader(trendingShowsJson));
            JsonSerializer seria = new JsonSerializer();
            JArray list = (JArray)seria.Deserialize(tr);

            JArray a = new JArray();
            JArray b = new JArray();
            list.Where(f => f["genres"].Values<string>().Contains(p)).All(p1 => { a.Add(p1); return true; });
            list.Where(f => !f["genres"].Values<string>().Contains(p)).All(p2 => { b.Add(p2); return true; });

            return new Tuple<string, string>(
                a.ToString(Formatting.None),
                b.ToString(Formatting.None)
            );
        }

        public static Tuple<string, string> SplitShowsByGenre5(string trendingShowsJson, string p)
        {
            JsonTextReader tr = new JsonTextReader(new StringReader(trendingShowsJson));
            JsonSerializer seria = new JsonSerializer();
            var list = seria.Deserialize<Film[]>(tr);

            return new Tuple<string, string>(
                JsonConvert.SerializeObject(list.Where(f => f.genres.Contains(p))),
                JsonConvert.SerializeObject(list.Where(f => !f.genres.Contains(p)))
            );
        }
        #endregion
    }
}
