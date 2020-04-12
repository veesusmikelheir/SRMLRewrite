using SRML.ModLoading.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.ModLoading.Core.Parsers
{
    public class AggregateModParser : IModParser
    {
        List<IModParser> Parsers = new List<IModParser>();

        public AggregateModParser(params IModParser[] parsers)
        {
            Parsers.AddRange(parsers);
        }

        public bool TryParse(IModFileSystem loadInfo, out IModInfo modInfo)
        {
            modInfo = null;
            foreach(var parser in Parsers)
            {
                if (parser.TryParse(loadInfo, out modInfo)) return true;
            }
            return false;
        }
    }
}
