﻿

namespace crazy_collects
{
    using System;
    using System.Runtime.Serialization;
    using System.Diagnostics.Contracts;
    using System.Collections.Generic;

    public enum ExceptionArgument
    {
        converter,
        item,
        capacity,
        dictionary,
        array,
        index,
        info,
        key,
        value,
        count,
        match,
        startIndex,
        collection,
    }
    public enum ExceptionResource
    {
        ArgumentOutOfRange_ListInsert,
        ArgumentOutOfRange_SmallCapacity,
        ArgumentOutOfRange_BiggerThanCollection,
        ArgumentOutOfRange_Count,
        ArgumentOutOfRange_Index,
        Argument_InvalidOffLen,
        NotSupported_ValueCollectionSet,
        NotSupported_KeyCollectionSet,
        ArgumentOutOfRange_NeedNonNegNum,
        Arg_ArrayPlusOffTooSmall,
        Argument_AddingDuplicate,
        Serialization_MissingKeys,
        Serialization_NullKey,
        Arg_NonZeroLowerBound,
        Arg_RankMultiDimNotSupported,
        Argument_InvalidArrayType,
        InvalidOperation_EnumFailedVersion,
        InvalidOperation_EnumOpCantHappen,
    }
    public class ThrowHelper
    {
        public static void ThrowArgumentOutOfRangeException()
        {

        }
        public static void ThrowArgumentOutOfRangeException(ExceptionArgument args, ExceptionResource args2)
        {

        }
        public static void ThrowArgumentOutOfRangeException(ExceptionArgument args)
        {

        }
        public static void ThrowArgumentNullException(ExceptionArgument args)
        {

        }
        public static void ThrowArgumentException(ExceptionResource args)
        {

        }
        public static void ThrowKeyNotFoundException()
        {

        }
        public static void ThrowSerializationException(ExceptionResource v)
        {

        }
        public static void IfNullAndNullsAreIllegalThenThrow<T>(object v, ExceptionArgument v2)
        {

        }
        public static void ThrowWrongValueTypeArgumentException(object v, System.Type v2)
        {

        }
        public static void ThrowWrongKeyTypeArgumentException(object v, System.Type v2)
        {

        }
        public static void ThrowInvalidOperationException(ExceptionResource v)
        {

        }
        public static void ThrowNotSupportedException(ExceptionResource v)
        {

        }
    }
    public class HashHelpers
    {
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                Contract.Assert(MaxPrimeArrayLength == GetPrime(MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
                return MaxPrimeArrayLength;
            }

            return GetPrime(newSize);
        }

        public static int GetPrime(int min)
        {
#if false
            if (min < 0)
                throw new ArgumentException(Environment.GetResourceString("Arg_HTCapacityOverflow"));
            Contract.EndContractBlock();
#else
            if (min < 0)
                throw new Exception("Arg_HTCapacityOverflow");
#endif

            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < System.Int32.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % Hashtable.HashPrime != 0))
                    return i;
            }
            return min;
        }
        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public class SerializationInfoTable
        {
            public static void TryGetValue(object v, out SerializationInfo siInfo)
            {
                siInfo = null;
            }
            public static void Remove(object v)
            {

            }
            public static void Add(Object obj, SerializationInfo info)
            {

            }
        }
    }
    public class Hashtable
    {
        internal const System.Int32 HashPrime = 101;
    }

    public class CrazyArray
    {
        internal const int MaxArrayLength = 0X7FEFFFFF;
        internal const int MaxByteArrayLength = 0x7FFFFFC7;

        internal sealed class FunctorComparer<T> : IComparer<T>
        {
            Comparison<T> comparison;

            public FunctorComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return comparison(x, y);
            }
        }
    }

    public static class BinaryCompatibility
    {
        public const bool TargetsAtLeast_Desktop_V4_5 = true;
    }

    public class SR
    {
        public enum ESr
        {
            Serialization_InvalidOnDeser,
            Invalid_Array_Type,
            Arg_NonZeroLowerBound,
            Arg_MultiRank,
            ExternalLinkedListNode,
            LinkedListNodeIsAttached,
            Serialization_MissingValues,
            LinkedListEmpty,
            IndexOutOfRange,
            Arg_InsufficientSpace,
            InvalidOperation_EnumOpCantHappen,
            InvalidOperation_EnumFailedVersion,
            Serialization_MissingKeys,
            Arg_HSCapacityOverflow,
            Arg_ArrayPlusOffTooSmall,
            ArgumentOutOfRange_NeedNonNegNum,
        }
        public static string GetString(ESr v)
        {
            return "";
        }
        public static string GetString(ESr v, int idx)
        {
            return "";
        }
    }

    public static class CrazyExtend
    {
        public static List<T> ToList<T>(this CrazyList<T> v)
        {
            return new List<T>(v.ToArray());
        }

        public static bool Remove<TKey, TValue>(
            this CrazyDictionary<TKey, TValue> dictionary,
            TKey key,
            out TValue value)
        {
            if (dictionary.TryGetValue(key, out value))
            {
                dictionary.Remove(key);
                
                return true;
            }

            return false;
        }
    }
}
