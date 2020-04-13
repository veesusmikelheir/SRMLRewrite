using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML.Config.Reflection
{
    /// <summary>
    /// A string parser that tries to auto-locate the Parse method for a given type
    /// </summary>
    public class ReflectionStringParser : IStringParser
    {
        Type ReflectedType;
        MethodInfo ParseMethod;

        public ReflectionStringParser(Type reflectedType) : this(reflectedType, GetParseMethod(reflectedType))
        {
        }

        public ReflectionStringParser(Type reflectedType, MethodInfo parseMethod)
        {
            ReflectedType = reflectedType;
            ParseMethod = parseMethod ?? throw new ArgumentException("parseMethod");
        }

        public object FromString(string str)
        {
            return ParseMethod.Invoke(null, new object[] { str });
        }

        public string ToString(object value)
        {

            if (!ReflectionUtil.IsCompatible(value, ReflectedType)) throw new ArgumentException("value");
            return value.ToString();
        }

        public static MethodInfo GetParseMethod(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            foreach (var method in type.GetMethods())
            {
                if (method.IsStatic && type.IsAssignableFrom(method.ReturnType) && method.GetParameters().Length == 1 && typeof(string).IsAssignableFrom(method.GetParameters()[0].ParameterType)) return method;
            }
            return null;
        }

        public static bool TryGetReflectionParser(Type type, out IStringParser parser)
        {
            parser = null;
            var method = GetParseMethod(type);
            if (method == null) return false;
            parser = new ReflectionStringParser(type, method);
            return true;
        }

        public class ReflectionParserGenerator : IParserGenerator
        {
            public bool TryGetParser(Type type, out IStringParser parser)
            {
                return TryGetReflectionParser(type, out parser);
            }
        }
    }
}
