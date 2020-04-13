using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Config
{
    public class ConfigFile : Dictionary<string,ConfigSection>
    {
        public void PullFrom(IniData data)
        {
            foreach(var v in data.Sections)
            {
                if (!TryGetValue(v.SectionName, out var value)) continue;
                value.PullFrom(v.Keys);
            }
        }

        public void PushTo(IniData data)
        {
            
            foreach(var section in this)
            {
                data.Sections.AddSection(section.Key); // make sure the section exists, if it does, this operation does nothing, so its fine
                section.Value.PushTo(data.Sections.GetSectionData(section.Key).Keys);
            }
        }

        public new ConfigSection this[string index]
        {
            get => base[index];
            set
            {
                base[index] = value;
                if (value == null) Remove(index);
            }
        }
    }
}
