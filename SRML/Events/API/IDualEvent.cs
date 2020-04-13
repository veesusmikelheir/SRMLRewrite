using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Events.API
{
    interface IDualEvent<T, K> where T : Delegate where K : Delegate
    {
        void RegisterPre(T callback);
        void RegisterPost(K callback);

        void UnregisterPre(T callback);

        void UnregisterPost(K callback);
    }
}
