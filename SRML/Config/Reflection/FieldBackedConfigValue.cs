using SRML.Config.Reflection.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SRML.Config.Reflection
{
    public class FieldBackedConfigValue : AbstractConfigValue
    {
        public override Type ValueType => IsStandIn ? StandIn.ValueType : field.FieldType;
        protected override object InternalValue
        {
            get
            {
                return IsStandIn ? StandIn.Value : field.GetValue(instance);
            }
            set
            {
                if (!IsStandIn) field.SetValue(instance, value);
                else StandIn.Value = value;
            }
        }

        bool IsStandIn => typeof(IValueStandIn).IsAssignableFrom(field.FieldType);
        IValueStandIn StandIn => ((IValueStandIn)field.GetValue(instance));
        FieldInfo field;
        object instance;
        IStringParser parser;

        public FieldBackedConfigValue(FieldInfo field, object instance)
        {
            this.field = field;
            this.instance = instance;
            parser = ParserManager.GetParser(ValueType);



        }


        string displayName;
        public string GetDisplayName()
        {
            if (displayName != null) return displayName;
            return displayName = field.GetCustomAttributes(false).Cast<ConfigNameAttribute>().FirstOrDefault()?.Name ?? field.Name;
        }

        public override string GetStringValue()
        {
            return parser.ToString(Value);
        }

        protected override object ParseString(string str)
        {
            return parser.FromString(str);
        }

    }


}
