using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Events.API
{
    public interface IDualEventTrigger<T, K> where T : Delegate where K : Delegate
    {
        T FirePre { get; }
        K FirePost { get; }
    }
}
