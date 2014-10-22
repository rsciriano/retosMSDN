using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShufflerOld
{
    public static class ListExtensions
    {
        public static List<int> Shuffle(this List<int> source)
        {
            if (source == null)
                return null;

            int length = source.Count;

            Random rnd = new Random();

            // Lista auxiliar con elementos pendientes de ordenar
            List<int> aux = Enumerable.Range(0, length).ToList();

            // Lista elementos reordenados
            List<int> shuffled = new List<int>(length);

            // Bucle por los elementos a ordenar
            for (int i = 0; i < length; i++)
            {
                // Elegir aleatoriamente un elemento de la lista pendiente
                int r = rnd.Next(length - i);

                // Obtener la posición original del elemento elegido eleatoriamente
                int pos = aux[r];

                if ( pos != i)
                {
                    // La posición original no coincide con la nueva, añadirlo a la lista
                    shuffled.Add(source[pos]);
                }
                else
                {
                    if (aux.Count > 1)
                    {
                        // La posición original coincide con la nueva y no es el último de la lista de pendientes, 
                        // tomar el elemento superior o inferior de los pendientes
                        if (r < aux.Count - 1)
                            r++;
                        else
                            r--;
                        pos = aux[r];

                        // Añadir elemento a la nueva lista
                        shuffled.Add(source[pos]);
                    }
                    else
                    {
                        // La posición original coincide con la nueva pero es el último de la lista de pendientes, 
                        // intercambiarlo por el ultimo en la lista reordenada
                        shuffled.Add(shuffled[i -1]);
                        shuffled[i - 1] = source[pos];
                    }
                }

                // Eliminar elemento de la lista de pendientes
                aux.RemoveAt(r);
            }

            return shuffled;
        }
    }
}
