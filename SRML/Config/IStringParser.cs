using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public interface IStringParser
    {
        object FromString(string str);
        string ToString(object value);
    }
}
