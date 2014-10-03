using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectDumper
{
    public class ObjectDumper<T> where T: class
    {
        Dictionary<string, Delegate> templates = new Dictionary<string,Delegate>();

        public IEnumerable<KeyValuePair<string, string>> Dump(object source)
        {
            if (source == null)
            {
                yield break;
            }
            else
            {
                // Bucle por las propiedades publicas del objeto
                foreach (var prop in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).OrderBy(p => p.Name))
                {
                    // Solo trabajar con propiedades que tienen método Get
                    if (prop.GetMethod != null)
                    {
                        // Buscar template para la propiedad
                        Delegate templateDelegate;
                        if (templates.TryGetValue(prop.Name, out templateDelegate))
                        {
                            // Aplicar template al valor
                            yield return new KeyValuePair<string, string>(prop.Name, (string) templateDelegate.DynamicInvoke(prop.GetValue(source)));
                        }
                        else
                        {
                            // Llamar a ToString (teniendo en cuenta el valor null)
                            yield return new KeyValuePair<string, string>(prop.Name, prop.GetValue(source) != null ? prop.GetValue(source).ToString() : null);
                        }
                    }
                }
            }
        }
        public void AddTemplateFor<TValue>(Expression<Func<T, TValue>> property, Func<TValue, string> value)
        {
            if (property != null)
            {
                // Obtener expresión pata poder extraer el nombre de la propiedad
                MemberExpression exp = property.Body as MemberExpression;
                
                // Almacenar la template en el diccionario  
                if (exp != null)
                    templates[exp.Member.Name] = value;
            }
        }
    }
}
