using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public class ArrayParserGenerator : IParserGenerator
    {
        public bool TryGetParser(Type type, out IStringParser parser)
        {
            parser = null;
            if (!type.IsArray) return false;
            var arrayType = type.GetElementType();

            var internalParser = ParserManager.GetParser(arrayType);
            parser = new ArrayStringParser(internalParser, arrayType);
            return true;
        }
    }
    public class ArrayStringParser : IStringParser
    {
        IStringParser InternalParser;
        Type ElementType;

        public ArrayStringParser(IStringParser internalParser, Type elementType)
        {
            InternalParser = internalParser;
            ElementType = elementType;
        }
        List<object> limbo = new List<object>(); // maybe make this static? it would prevent multithreading but it would give a smaller memory footprint, idk atm
        public object FromString(string str)
        {
            str = str.Trim('\r', '\n', ' ', '[', ']');

            var values = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            limbo.Clear(); // the intermediate list is so we can make sure every element deserializes correctly before making the array
            foreach(string value in values)
            {
                limbo.Add(InternalParser.FromString(value.Trim()));
            }
            var array = Array.CreateInstance(ElementType, limbo.Count);
            for(int i = 0; i < limbo.Count; i++)
            {
                array.SetValue(limbo[i], i);
            }
            return array;
        }

        public string ToString(object value)
        {
            if (!(value is Array array) || !ElementType.IsAssignableFrom(array.GetType().GetElementType())) throw new ArgumentException("value");
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            for(int i = 0;i<array.Length;i++)
            {
                if (i > 0) builder.Append(", ");
                builder.Append(InternalParser.ToString(array.GetValue(i)));
                
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}
