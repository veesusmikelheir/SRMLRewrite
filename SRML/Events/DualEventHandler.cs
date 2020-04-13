using SRML.Events.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Events
{
    public class DualEventHandler<T, K> : IDualEventHandler<T, K> where T : Delegate where K : Delegate
    {
        public virtual T FirePre { get; protected set; }

        public virtual K FirePost { get; protected set; }

        public virtual void RegisterPost(K callback)
        {
            FirePost = (K)Delegate.Combine(FirePre, callback);
        }

        public virtual void RegisterPre(T callback)
        {
            FirePre = (T)Delegate.Combine(FirePost, callback);
        }

        public virtual void UnregisterPost(K callback)
        {
            FirePost = (K)Delegate.Remove(FirePost, callback);
        }

        public virtual void UnregisterPre(T callback)
        {
            FirePre = (T)Delegate.Remove(FirePre, callback);

        }
    }
}
