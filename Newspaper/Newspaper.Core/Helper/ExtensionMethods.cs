using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Newspaper.Core.Helper
{
    public static class ExtensionMethods
    {
        public static string ToJsonNS(this Object obj, bool handleRefLoop = true)
        {
            return Helper.ToJsonNS(obj, handleRefLoop);
        }
        public static bool IsNullOrEmptyWithTrim(this string str)
        {

            if (str == null || str.Trim() == "")
                return true;
            return false;

        }
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> source, string property)
        {
            return ApplyOrder<T>(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(IQueryable<T> source, string property, string methodName)
        {
            try
            {
                if (property.Contains("."))//complex type nested
                {
                    StringBuilder sb = new StringBuilder(property);
                    sb[0] = Char.ToUpper(property[0]);
                    sb[property.IndexOf('.') + 1] = Char.ToUpper(property[property.IndexOf('.') + 1]);
                    property = sb.ToString();
                }
                else
                {
                    property = char.ToUpper(property[0]) + property.Substring(1);
                }
                string[] props = property.Split('.');
                Type type = typeof(T);
                ParameterExpression arg = Expression.Parameter(type, "x");
                Expression expr = arg;
                foreach (string prop in props)
                {
                    // use reflection (not ComponentModel) to mirror LINQ
                    PropertyInfo pi = type.GetProperty(prop);
                    expr = Expression.Property(expr, pi);
                    type = pi.PropertyType;
                }
                Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
                LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

                object result = typeof(Queryable).GetMethods().Single(
                        method => method.Name == methodName
                                && method.IsGenericMethodDefinition
                                && method.GetGenericArguments().Length == 2
                                && method.GetParameters().Length == 2)
                        .MakeGenericMethod(typeof(T), type)
                        .Invoke(null, new object[] { source, lambda });
                return (IOrderedQueryable<T>)result;

            }
            catch (Exception)
            {
                return source.OrderBy<T>("UID");
            }

        }

    }
}
