using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public delegate void ValueChangedDelegate(object oldValue, object newValue);
    public interface IConfigValue
    {
        object Value { get; set; }
        Type ValueType { get; }
        string GetStringValue();
        void SetStringValue(string str);

        event ValueChangedDelegate OnValueChanged;
    }
}
