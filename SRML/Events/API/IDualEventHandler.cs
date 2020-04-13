using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Events.API
{
    interface IDualEventHandler<T, K> : IDualEventTrigger<T,K>, IDualEvent<T,K> where T : Delegate where K : Delegate
    {
    }
}
