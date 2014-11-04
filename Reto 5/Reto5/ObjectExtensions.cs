using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reto5
{
    public static class ObjectExtensions
    {
        unsafe public static void ToUpperNoCopy(this string source)
        {
            if (source == null)
            {
                throw new ArgumentNullException();
            }
            else
            {
                fixed (char* c = source)
                {
                    char* aux = c; 
                    for (int i = 0; i < source.Length; i++)
                    {
                        *aux = Convert.ToString(*aux).ToUpper()[0];
                        aux++;
                    }                    
                }
            }
        }
        public static T NotNull<T>(this T source, string name = null) where T: class
        {
            if (source == null)
            {
                if (name != null)
                    throw new ArgumentNullException(
                        string.Format("TestNotNullWithNull{0}{1} was called with null parameter\r\nParameter name: {2}", 
                            typeof(T).Name,
                            typeof(T) == typeof(String) ? "" : "Object",
                            name)
                        , (Exception)null);
                else 
                    throw new ArgumentNullException();
            }
            else
            {
                return source;
            }
        }

        public static DateTime From(this Duration duration, DateTime dt)
        {
            switch (duration)
            {
                case Duration.Day:
                    return dt.AddDays(1);                    
                case Duration.Week:
                    return dt.AddDays(7);
                case Duration.Month:
                    return dt.AddMonths(1);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
