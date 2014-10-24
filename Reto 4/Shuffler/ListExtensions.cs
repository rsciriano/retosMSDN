using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuffler
{
    public static class ListExtensions
    {
        public static List<T> Shuffle<T>(this List<T> source)
        {
            if (source == null)
                return null;

            int length = source.Count;

            Random rnd = new Random();

            // Lista auxiliar con orden generado aleatotiamente
            IEnumerable<int> aux = Enumerable.Range(0, length).OrderBy(v => rnd.Next());

            // Lista elementos reordenados
            List<T> shuffled = new List<T>(length);

            // Variable para almacenar elemento que coincide con posición original
            int? pending = null;

            // Bucle por evitar que un elemento coincida con su posición original
            foreach (int i in aux)
            {
                if (i != shuffled.Count)
                {
                    // Añadir nuevo elemento a la lista aleatoria
                    shuffled.Add(source[i]);

                    // Añadir elemento pendiente de iteración anterior si lo había
                    if (pending.HasValue)
                    {
                        shuffled.Add(source[pending.Value]);
                        pending = null;
                    }
                }
                else
                {
                    // Comprobación de que solo hay un elemento pendiente
                    Debug.Assert(!pending.HasValue);
                    
                    // Guardar el elemento pendiente
                    pending = i;
                }
            }
            return shuffled;
        }
    }
}
