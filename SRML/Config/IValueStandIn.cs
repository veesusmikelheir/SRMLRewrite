using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public interface IValueStandIn
    {
        object Value { get; set; }
        Type ValueType { get; }
    }

    public abstract class ValueStandIn<T> : IValueStandIn
    {
        object IValueStandIn.Value
        {
            get => Value;
            set
            {
                if (!ReflectionUtil.IsCompatible(value, ((IValueStandIn)this).ValueType)) throw new InvalidCastException($"Cannot cast {value.GetType()} to {((IValueStandIn)this).ValueType}");
                Value = (T)value;
            }
        }

        public abstract T Value { get; set; }
        Type IValueStandIn.ValueType => typeof(T);

        public static explicit operator T(ValueStandIn<T> value)=>value.Value;
    }
}
