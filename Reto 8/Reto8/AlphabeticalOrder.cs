using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto8
{
    public class AlphabeticalOrder
    {
        /// <summary>
        /// Comparador de la primera fase: comparar solo los primeros caracteres de la cadena mas corta 
        /// </summary>
        public class LexicographicalStringComparer_Phase1 : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == null || y == null)
                    throw new ArgumentNullException();

                int min = Math.Min(x.Length, y.Length);                
                int r = 0;
                for (int i = 0; i < min; i++)
                {
                    r = x[i].CompareTo(y[i]);
                    if (r != 0)
                        break;
                }
                return r;
            }
        }

        /// <summary>
        /// Comparador segunda fase: comtarar cadenas concatenadas
        /// </summary>
        public class LexicographicalStringComparer_Phase2 : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                if (x == null || y == null)
                    throw new ArgumentNullException();

                return (x + y).CompareTo(y + x);
            }
        }


        public static string GetShortestConcatString(string str)
        {
            if (str == null)
                throw new ArgumentNullException();

            string[] list = str.Split(' ');

            // Ordenación en dos fases
            var sortedList = list
                .OrderBy(s => s, new LexicographicalStringComparer_Phase1())
                .ThenBy(s => s, new LexicographicalStringComparer_Phase2());

            return string.Join("", sortedList);
        }
        public static IEnumerable<string> GetShortestConcatString(params string[] strs)
        {
            if (strs == null)
                throw new ArgumentNullException();

            return GetShortestConcatString(strs.AsEnumerable());
        }
        public static IEnumerable<string> GetShortestConcatString(IEnumerable<string> strs)
        {
            if (strs == null)
                throw new ArgumentNullException();

            List<string> result = new List<string>();
            foreach (var s in strs)
            {
                result.Add(GetShortestConcatString(s));
            }
            return result;
        }
    }
}
