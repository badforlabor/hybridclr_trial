using System;
using System.Collections.Generic;
using UnityEngine;

namespace my_generic
{
    
    public class ReusableDict<KKKK, VVVV>
    {
        public struct KeyValuePair
        {
            public KKKK Key;
            public VVVV Value;

            public KeyValuePair(KKKK key, VVVV value)
            {
                Key = key;
                Value = value;
            }
        }


        private PoolArray<KKKK> KeyList;
        private PoolArray<VVVV> ValueList;
        private bool Lock;

        public ReusableDict(int capacity = 32)
        {
            KeyList = new PoolArray<KKKK>(capacity);
            ValueList = new PoolArray<VVVV>(capacity);
        }

        public void Release()
        {
            if (KeyList != null)
            {
                KeyList.Release();
                KeyList = null;
                ValueList.Release();
                ValueList = null;   
            }
        }
        
        public void Clear()
        {
            KeyList.Clear();
            ValueList.Clear();
        }

        public VVVV this[in KKKK key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public int Count => KeyList.Count;
        public int Capacity => KeyList.Capacity;

        public bool Contains(in KKKK key)
        {
            return KeyList.Contains(key);
        }

        public void Add(in KKKK key, VVVV v)
        {
            SetValue(key, v);
        }

        private VVVV GetValue(in KKKK key)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {
                return ValueList[idx];
            }

            return default;
        }

        void SetValue(in KKKK key, in VVVV v)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {

                KeyList[idx] = key;
                ValueList[idx] = v;
                return;
            }
            
            KeyList.Add(key);
            ValueList.Add(v);
        }

        public bool Remove(in KKKK key)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {
                KeyList.RemoveAt(idx);
                ValueList.RemoveAt(idx);
                
                return true;
            }

            return false;
        }

        public bool Remove(in KKKK key, out VVVV value)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {
                value = ValueList[idx];
                
                KeyList.RemoveAt(idx);
                ValueList.RemoveAt(idx);

                return true;
            }

            value = default;
            return false;
        }

        public bool TryGetValue(in KKKK key, out VVVV v)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {
                v = ValueList[idx];
                return true;
            }

            v = default;
            return false;
        }


        public Enumerator GetEnumerator ()
        {
            // return obj.Items.GetEnumerator();
            return new Enumerator(this);
        }
        
        public struct Enumerator : IDisposable {
            readonly ReusableDict<KKKK, VVVV> _filter;
            readonly int _count;
            int _idx;


            internal Enumerator (ReusableDict<KKKK, VVVV> filter) {
                _filter = filter;
                _count = _filter.KeyList.Count;
                _idx = -1;
                _filter.Lock = true;
            }

            public KeyValuePair Current {
                get
                {
                    var kv = new KeyValuePair(_filter.KeyList[_idx], _filter.ValueList[_idx]);
                    return kv;
                }
            }

            
            public void Dispose () {
                _filter.Lock = false;
            }

            public bool MoveNext () {
                return ++_idx < _count;
            }

            public void Reset()
            {
            }
        }
    }
    
    
    public class PoolArray<TTTT>
    {
        protected TArray<TTTT> obj;
        private bool Lock;
        private bool bDirty;


        public void ClearDirty()
        {
        }

        public bool IsDirty()
        {
            return bDirty;
        }

        public PoolArray(int capacity = 32)
        {
            Resize(capacity);
            bDirty = true;
        }

        public void Release()
        {
            if (obj != null)
            {
                SetCount(0);
                obj.Release();
                obj = null;
            }
        }

        public void Clear()
        {
            SetCount(0);
        }

        public TTTT this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }
        // public TTTT First => GetValue(0);

        public int Count => obj.Count;
        public int Length => Count;

        public int Capacity
        {
            get
            {
                Debug.Assert(obj != null);
                return obj.Items.Length;
            }
        }

        private TTTT GetValue(int key)
        {
            return obj.Items[key];
        }

        void SetValue(int key, in TTTT v)
        {
            Debug.Assert(key < Count);

            // if (!obj.Items[key].Equals(v))
#if MARK_DIRTY
            if(!MyHashTool.DoCompare(v, obj.Items[key]))
            {
                bDirty = true;
            }
#endif

            obj.Items[key] = v;
        }

        public void SetCount(int count)
        {
            // todo.
            for (int i = count - 1; i < Count; i++)
            {
                if (i < 0)
                {
                    continue;
                }
                // MyHashTool.SetDefault(null, ref obj.Items[i]);
            }

            if (count >= obj.Items.Length)
            {
                int cap = obj.Items.Length;
                while (cap < count)
                {
                    cap *= 2;
                }

                Resize(cap);
            }

            obj.Count = count;
            bDirty = true;
        }

        public void AddAt(int idx, in TTTT v)
        {
            Debug.Assert(!Lock);

            if (obj.Count >= obj.Items.Length)
            {
                Resize(obj.Items.Length * 2);
            }

            obj.Count++;
            bDirty = true;

            for (int i = idx + 1; i < Count; i++)
            {
                obj.Items[i] = obj.Items[i - 1];
            }

            obj.Items[idx] = v;
        }

        public void Add(in TTTT v)
        {
            Debug.Assert(!Lock);

            if (obj.Count >= obj.Items.Length)
            {
                Resize(obj.Items.Length * 2);
            }

            obj.Items[obj.Count] = v;
            obj.Count++;
            bDirty = true;
        }

        public void AddLast(in TTTT v)
        {
            Add(v);
        }


        public bool Remove(in System.Predicate<TTTT> match)
        {
            Debug.Assert(!Lock);

            var idx = Find(match);
            if (idx >= 0)
            {
                RemoveAt(idx);

                return true;
            }

            return false;
        }

        public bool Remove(in TTTT v)
        {
            Debug.Assert(!Lock);

            var idx = Find(v);

            if (idx >= 0)
            {
                RemoveAt(idx);

                return true;
            }

            return false;
        }

        public void RemoveAt(int idx)
        {
            if (idx >= 0)
            {
                Debug.Assert(idx < Count);

                for (int i = idx; i < obj.Items.Length - 1; i++)
                {
                    obj.Items[i] = obj.Items[i + 1];
                }
// #if DEBUG
                // obj.Items[obj.Items.Length - 1] = default;
                // MyHashTool.SetDefault(null, ref obj.Items[obj.Items.Length - 1]);
// #endif

                obj.Count--;
            }
        }

        public bool Contains(in TTTT v)
        {
            return Find(v) >= 0;
        }

        public int Find(in TTTT v)
        {
            for (int i = 0; i < obj.Items.Length; i++)
            {
                if (obj.Items[i].Equals(v))
                {
                    return i;
                }
            }

            return -1;
        }

        public int Find(in System.Predicate<TTTT> match)
        {
            for (int i = 0; i < Count; i++)
            {
                if (match(obj.Items[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public class LinkEnum
        {
            private PoolArray<TTTT> owner;
            internal int Idx;

            internal LinkEnum(PoolArray<TTTT> filter, int idx)
            {
                owner = filter;
                Idx = idx;
            }

            public TTTT Value
            {
                get { return owner.GetValue(Idx); }
            }

            public LinkEnum Previous
            {
                get
                {
                    if (Idx <= 0)
                    {
                        return null;
                    }

                    return new LinkEnum(owner, Idx - 1);
                }
            }

            public LinkEnum Next
            {
                get
                {
                    if (Idx + 1 >= owner.Count)
                    {
                        return null;
                    }

                    return new LinkEnum(owner, Idx + 1);
                }
            }
        }

        public LinkEnum First
        {
            get
            {
                if (Count <= 0)
                {
                    return null;
                }

                return new LinkEnum(this, 0);
            }
        }

        public LinkEnum Last
        {
            get
            {
                if (Count <= 0)
                {
                    return null;
                }

                return new LinkEnum(this, Count - 1);
            }
        }

        public void AddBefore(LinkEnum idx, in TTTT v)
        {
            AddAt(idx.Idx, v);
        }

        public Enumerator GetEnumerator()
        {
            // return obj.Items.GetEnumerator();
            return new Enumerator(this);
        }

        public struct Enumerator : IDisposable
        {
            readonly PoolArray<TTTT> _filter;
            readonly int _count;
            int _idx;


            internal Enumerator(PoolArray<TTTT> filter)
            {
                _filter = filter;
                _count = _filter.Count;
                _idx = -1;
                _filter.Lock = true;
            }

            public TTTT Current
            {
                get => _filter.GetValue(_idx);
            }


            public void Dispose()
            {
                _filter.Lock = false;
            }

            public bool MoveNext()
            {
                return ++_idx < _count;
            }

            public void Reset()
            {
            }
        }


        void Resize(int capacity)
        {
            var old1 = obj;

            obj = TArray<TTTT>.New(capacity);

            if (old1 != null)
            {
                Array.Copy(old1.Items, obj.Items, old1.Count);
                old1.Release();
            }

            bDirty = true;
        }

        protected void ResizeWithNoCopy(int capacity)
        {
            var old1 = obj;

            obj = TArray<TTTT>.New(capacity);

            if (old1 != null)
            {
                old1.Release();
            }

            bDirty = true;
        }
    }


    public class TArray<TTTT>
    {
        public static TArray<TTTT> New(int capacity)
        {
            var obj = new TArray<TTTT>();
            return obj;
        }

        private static TArray<TTTT> InstanceFunc(object condition)
        {
            if (condition == null)
            {
                return new TArray<TTTT>();
            }
            else
            {
                var args = condition is int ? (int) condition : 32;
                return new TArray<TTTT>(args);
            }
        }

        public void Delete()
        {
        }

        public void Release()
        {
            Delete();
        }


        public TTTT[] Items;
        public int Count;

#if DEBUG
        private int uuid;
#endif

        protected TArray() : this(32)
        {
        }

        protected TArray(int capacity)
        {
            Items = new TTTT[capacity];
            Count = 0;
        }
    }
}