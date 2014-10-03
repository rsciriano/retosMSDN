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
                foreach (var prop in source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty).OrderBy(p => p.Name))
                {
                    if (prop.GetMethod != null)
                    {
                        Delegate templateDelegate;
                        if (templates.TryGetValue(prop.Name, out templateDelegate))
                        {
                            yield return new KeyValuePair<string, string>(prop.Name, (string) templateDelegate.DynamicInvoke(prop.GetValue(source)));
                        }
                        else
                        {
                            yield return new KeyValuePair<string, string>(prop.Name, prop.GetValue(source).ToString());
                        }
                    }
                }
            }
        }
        public void AddTemplateFor<TValue>(Expression<Func<T, TValue>> property, Func<TValue, string> value)
        {
            if (property != null)
            {
                MemberExpression exp = property.Body as MemberExpression;
                if (exp != null)
                    templates.Add(exp.Member.Name, value);
            }
        }
    }
}
