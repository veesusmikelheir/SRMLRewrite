using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public interface IParserGenerator
    {
        bool TryGetParser(Type type, out IStringParser parser);
    }
}
