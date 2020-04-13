using SRML.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SRML.Config
{
    public class Range<T> : ValueStandIn<T>
    {
        T internalValue;
        public override T Value
        {
            get => internalValue;
            set
            {
                if (MinChecker(value, MinBound)) value = MinBound;
                else if (MaxChecker(value, MaxBound)) value = MaxBound;
                internalValue = value;
            }
        }

        public T MinBound { get; protected set; }
        public T MaxBound { get; protected set; }

        Func<T, T, bool> MaxChecker;
        Func<T, T, bool> MinChecker;

        public Range(T startingValue, T minBound, T maxBound) : this(startingValue,minBound,maxBound,ReflectionUtil.GetGreaterThan<T>(),ReflectionUtil.GetLessThan<T>())
        {

        }

        public Range(T startingValue, T minBound, T maxBound, Func<T, T, bool> maxChecker, Func<T, T, bool> minChecker) 
        {
            this.internalValue = startingValue;
            MinBound = minBound;
            MaxBound = maxBound;
            MaxChecker = ReflectionUtil.GetGreaterThan<T>();
            MinChecker = ReflectionUtil.GetLessThan<T>();
        }

        public Range(T minBound, T maxBound) : this(minBound,minBound,maxBound)
        {

        }


    }
}
