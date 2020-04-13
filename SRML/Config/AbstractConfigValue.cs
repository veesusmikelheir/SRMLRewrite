using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public abstract class AbstractConfigValue : IConfigValue
    {

        protected abstract object InternalValue { get; set; }
        public object Value
        {
            get => InternalValue;
            set
            {
                if (!ReflectionUtil.IsCompatible(value,ValueType)) throw new InvalidCastException($"Cannot cast {value?.GetType().ToString() ?? "null"} to {ValueType}");
                var oldValue = InternalValue;
                InternalValue = value;
                OnValueChanged?.Invoke(oldValue, value);
                
            }
        }

        public abstract Type ValueType { get; }
        

        public event ValueChangedDelegate OnValueChanged;

        protected abstract object ParseString(string str);

        public abstract string GetStringValue();

        public void SetStringValue(string str)
        {
            Value = ParseString(str);
            
        }
    }
}
