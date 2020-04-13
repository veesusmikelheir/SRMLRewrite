using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SRML.Config.Reflection.ReflectionStringParser;

namespace SRML.Config
{
    public static class ParserManager
    {
        static Dictionary<Type, IStringParser> Parsers = new Dictionary<Type, IStringParser>();
        static List<IParserGenerator> Generators = new List<IParserGenerator>();
        

        // initialize basic parsers and generators
        static ParserManager()
        {
            Parsers[typeof(string)] = new StringStringParser();
            Generators.Add(new ReflectionParserGenerator());
            Generators.Add(new ArrayParserGenerator());
        }
        /// <summary>
        /// Register a <see cref="IParserGenerator"/> to be used in <see cref="GetParser(Type)"/>
        /// </summary>
        /// <param name="generator"></param>
        public static void RegisterParserGenerator(IParserGenerator generator)
        {
            Generators.Add(generator);
        }

        /// <summary>
        /// Get a parser for the given type. Throws an exception for types that can't be parsed
        /// </summary>
        /// <param name="type">Type to get the parser for</param>
        /// <returns>Parser for given type</returns>
        public static IStringParser GetParser(Type type)
        {
            if (Parsers.TryGetValue(type, out var parser)) return parser;
            parser = GenerateParser(type);
            Parsers[type] = parser ?? throw new ArgumentOutOfRangeException("type",$"Can't get parser for {type}");
            return parser;
        }

        static IStringParser GenerateParser(Type type)
        {
            foreach(var v in Generators)
            {
                if (v.TryGetParser(type, out var parser)) return parser;
            }
            return null;
        }
    }
}
