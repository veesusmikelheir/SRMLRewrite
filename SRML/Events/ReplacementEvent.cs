using SRML.Events.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SRML.Events
{
    public delegate void PreReplacementDelegate<T, K>(T caller,ref K informationValue);
    public delegate void PostReplacementDelegate<T, K>(T caller, K informationValue);
    public class ReplacementEvent<T, K> : DualEventHandler<PreReplacementDelegate<T, K>, PostReplacementDelegate<T, K>> where K : struct
    {
        Dictionary<T, IDualEventTrigger<SimplePreReplacement<K>, SimplePostReplacement<K>>> triggerers = new Dictionary<T, IDualEventTrigger<SimplePreReplacement<K>, SimplePostReplacement<K>>>();
        public IDualEventTrigger<SimplePreReplacement<K>,SimplePostReplacement<K>> GetSingleTargetTrigger(T target)
        {
            if(!triggerers.TryGetValue(target,out var trigger))
            {
                trigger = new SingleTargetReplacementEventTrigger<T, K>(target, this);
                triggerers[target] = trigger;
            }
            return trigger;
        }
        
    }
    public delegate void SimplePreReplacement<K>(ref K informationValue);
    public delegate void SimplePostReplacement<K>(K informationValue);
    class SingleTargetReplacementEventTrigger<T,K> : IDualEventTrigger<SimplePreReplacement<K>,SimplePostReplacement<K>> where K : struct
    {
        T target;
        ReplacementEvent<T, K> provider;
        public SingleTargetReplacementEventTrigger(T target, ReplacementEvent<T,K> provider)
        {
            this.target = target;
            this.provider = provider;
        }

        public SimplePreReplacement<K> FirePre =>(ref K x) => provider.FirePre(target,ref x);

        public SimplePostReplacement<K> FirePost => (x => provider.FirePost(target, x));
    }
}
