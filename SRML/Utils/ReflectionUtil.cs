using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SRML.Utils
{
    public static class ReflectionUtil
    {
        public static bool IsCompatible(object value, Type type)
        {
            return value == null ? (type.IsValueType ? false : true) : type.IsAssignableFrom(value.GetType());
        }

        public static Func<T, T, bool> GetGreaterThan<T>()
        {
            var param1 = Expression.Parameter(typeof(T), "param1");
            var param2 = Expression.Parameter(typeof(T), "param2");
            var binaryExpression = Expression.GreaterThan(param1, param2);
            return Expression.Lambda<Func<T, T, bool>>(binaryExpression, param1, param2).Compile();
        }

        public static Func<T, T, bool> GetLessThan<T>()
        {
            var param1 = Expression.Parameter(typeof(T), "param1");
            var param2 = Expression.Parameter(typeof(T), "param2");
            var binaryExpression = Expression.LessThan(param1, param2);
            return Expression.Lambda<Func<T, T, bool>>(binaryExpression, param1, param2).Compile();
        }
    }
}
