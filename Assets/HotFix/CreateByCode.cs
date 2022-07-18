using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SDataC
{
    public int A;
    public float B;
    public string C;
    public float B2;
}
public class CDataC
{
    public int A;
    public float B;
    public string C;
    public float B2;
}

public class CreateByCode : MonoBehaviour
{
    struct SDataA
    {
        public int A;
        public float B;
        public string C;
    }
    class CDataA
    {
        public int A;
        public float B;
        public string C;
    }
    struct SDataB
    {
        public int A;
        public float B;
        public string C;
        public float B2;
        public float B3;
        public float B4;
        public float B5;
        public float B6;
        public float B7;
        public float B8;
        public float B9;
        public float B10;
    }
    class CDataB
    {
        public int A;
        public float B;
        public string C;
        public float B2;
        public float B3;
        public float B4;
        public float B5;
        public float B6;
        public float B7;
        public float B8;
        public float B9;
        public float B10;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("zzz 这个脚本是通过代码AddComponent直接创建的");
        TestAOTGeneric();
        TestAOTGeneric2();
        TestAOTGeneric3();
        TestAOTGeneric4();
        TestAOTGeneric5();
        TestAOTGeneric6();
        TestAOTGeneric7();
        TestAOTGeneric8();
        
        TestAOTGeneric9();
        TestAOTGeneric10();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestAOTGeneric()
    {
        try
        {
            var dict = new Dictionary<string, SDataC>();
            dict["123"] = new SDataC() {A = 1, B = 2, C = "123"};
            Debug.Log($"AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

        TestAOTGeneric1_1();
        TestAOTGeneric1_2();
        TestAOTGeneric1_3();
    }
    public void TestAOTGeneric1_1()
    {
        try
        {
            var dict = new my_generic.PoolArray_SDataC();
            dict.Add(new SDataC() {A = 1, B = 2, C = "123"});
            Debug.Log($"1_1 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric1_2()
    {
        try
        {
            var dict = new my_generic.Dict_string_SDataC();
            dict["123"] = new SDataC() {A = 1, B = 2, C = "123"};
            Debug.Log($"1_2 AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric1_3()
    {
        try
        {
            var dict = new SDataC[10];
            dict[0] = (new SDataC() {A = 1, B = 2, C = "123"});
            Debug.Log($"1_3 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric2()
    {
        try
        {
            var dict = new Dictionary<string, CDataC>();
            dict["123"] = new CDataC() {A = 1, B = 2, C = "123"};
            Debug.Log($"2 AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric3()
    {
        try
        {
            var dict = new Dictionary<string, SDataB>();
            dict["123"] = new SDataB() {A = 1, B = 2, C = "123"};
            Debug.Log($"3 AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric4()
    {
        try
        {
            var dict = new Dictionary<string, CDataB>();
            dict["123"] = new CDataB() {A = 1, B = 2, C = "123"};
            Debug.Log($"4 AOT泛型补充元数据机制测试正常, A=1=={dict["123"].A}, B=2=={dict["123"].B}, C='123'=={dict["123"].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric5()
    {
        try
        {
            var dict = new List<SDataB>();
            dict.Add(new SDataB() {A = 1, B = 2, C = "123"});
            Debug.Log($"5 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric6()
    {
        try
        {
            var dict = new List<CDataB>();
            dict.Add(new CDataB() {A = 1, B = 2, C = "123"});
            Debug.Log($"6 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric7()
    {
        try
        {
            var dict = new List<SDataA>();
            dict.Add(new SDataA() {A = 1, B = 2, C = "123"});
            Debug.Log($"7 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric8()
    {
        try
        {
            var dict = new List<CDataA>();
            dict.Add(new CDataA() {A = 1, B = 2, C = "123"});
            Debug.Log($"8 AOT泛型补充元数据机制测试正常, A=1=={dict[0].A}, B=2=={dict[0].B}, C='123'=={dict[0].C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric9()
    {
        try
        {
            var dict = new HashSet<SDataB>();
            dict.Add(new SDataB() {A = 1, B = 2, C = "123"});
            var it = dict.GetEnumerator();
            it.MoveNext();
            Debug.Log($"9 AOT泛型 HashSet<SDataB>, A=1=={it.Current.A}, B=2=={it.Current.B}, C='123'=={it.Current.C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void TestAOTGeneric10()
    {
        try
        {
            var dict = new HashSet<CDataB>();
            dict.Add(new CDataB() {A = 1, B = 2, C = "123"});
            var it = dict.GetEnumerator();
            it.MoveNext();
            Debug.Log($"10 AOT泛型 HashSet<CDataB>, A=1=={it.Current.A}, B=2=={it.Current.B}, C='123'=={it.Current.C}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
namespace my_generic
{
    public class Dict_string_SDataC
    {
        public struct KeyValuePair
        {
            public string Key;
            public SDataC Value;

            public KeyValuePair(string key, SDataC value)
            {
                Key = key;
                Value = value;
            }
        }


        private PoolArray_string KeyList;
        private PoolArray_SDataC ValueList;
        private bool Lock;

        public Dict_string_SDataC(int capacity = 32)
        {
            KeyList = new PoolArray_string(capacity);
            ValueList = new PoolArray_SDataC(capacity);
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

        public SDataC this[in string key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public int Count => KeyList.Count;
        public int Capacity => KeyList.Capacity;

        public bool Contains(in string key)
        {
            return KeyList.Contains(key);
        }

        public void Add(in string key, SDataC v)
        {
            SetValue(key, v);
        }

        private SDataC GetValue(in string key)
        {
            var idx = KeyList.Find(key);
            if (idx >= 0)
            {
                return ValueList[idx];
            }

            return default;
        }

        void SetValue(in string key, in SDataC v)
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

        public bool Remove(in string key)
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

        public bool Remove(in string key, out SDataC value)
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

        public bool TryGetValue(in string key, out SDataC v)
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
            readonly Dict_string_SDataC _filter;
            readonly int _count;
            int _idx;


            internal Enumerator (Dict_string_SDataC filter) {
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
        
    public class PoolArray_string
    {
        protected TArray_string obj;
        private bool Lock;
        private bool bDirty;


        public void ClearDirty()
        {
        }

        public bool IsDirty()
        {
            return bDirty;
        }

        public PoolArray_string(int capacity = 32)
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

        public string this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }
        // public string First => GetValue(0);

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

        private string GetValue(int key)
        {
            return obj.Items[key];
        }

        void SetValue(int key, in string v)
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

        public void AddAt(int idx, in string v)
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

        public void Add(in string v)
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

        public void AddLast(in string v)
        {
            Add(v);
        }


        public bool Remove(in System.Predicate<string> match)
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

        public bool Remove(in string v)
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

        public bool Contains(in string v)
        {
            return Find(v) >= 0;
        }

        public int Find(in string v)
        {
            for (int i = 0; i < obj.Items.Length; i++)
            {
                if (obj.Items[i] != null && obj.Items[i].Equals(v))
                {
                    return i;
                }
            }

            return -1;
        }

        public int Find(in System.Predicate<string> match)
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
            private PoolArray_string owner;
            internal int Idx;

            internal LinkEnum(PoolArray_string filter, int idx)
            {
                owner = filter;
                Idx = idx;
            }

            public string Value
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

        public void AddBefore(LinkEnum idx, in string v)
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
            readonly PoolArray_string _filter;
            readonly int _count;
            int _idx;


            internal Enumerator(PoolArray_string filter)
            {
                _filter = filter;
                _count = _filter.Count;
                _idx = -1;
                _filter.Lock = true;
            }

            public string Current
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

            obj = TArray_string.New(capacity);

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

            obj = TArray_string.New(capacity);

            if (old1 != null)
            {
                old1.Release();
            }

            bDirty = true;
        }
	}
    
    public class TArray_string
    {
        public static TArray_string New(int capacity)
        {
            var obj = new TArray_string();
            return obj;
        }

        private static TArray_string InstanceFunc(object condition)
        {
            if (condition == null)
            {
                return new TArray_string();
            }
            else
            {
                var args = condition is int ? (int) condition : 32;
                return new TArray_string(args);
            }
        }

        public void Delete()
        {
        }

        public void Release()
        {
            Delete();
        }


        public string[] Items;
        public int Count;

#if DEBUG
        private int uuid;
#endif

        protected TArray_string() : this(32)
        {
        }

        protected TArray_string(int capacity)
        {
            Items = new string[capacity];
            Count = 0;
        }
    }
    
    
    public class PoolArray_SDataC
    {
        protected TArray_SDataC obj;
        private bool Lock;
        private bool bDirty;


        public void ClearDirty()
        {
        }

        public bool IsDirty()
        {
            return bDirty;
        }

        public PoolArray_SDataC(int capacity = 32)
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

        public SDataC this[int key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }
        // public SDataC First => GetValue(0);

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

        private SDataC GetValue(int key)
        {
            return obj.Items[key];
        }

        void SetValue(int key, in SDataC v)
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

        public void AddAt(int idx, in SDataC v)
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

        public void Add(in SDataC v)
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

        public void AddLast(in SDataC v)
        {
            Add(v);
        }


        public bool Remove(in System.Predicate<SDataC> match)
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

        public bool Remove(in SDataC v)
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

        public bool Contains(in SDataC v)
        {
            return Find(v) >= 0;
        }

        public int Find(in SDataC v)
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

        public int Find(in System.Predicate<SDataC> match)
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
            private PoolArray_SDataC owner;
            internal int Idx;

            internal LinkEnum(PoolArray_SDataC filter, int idx)
            {
                owner = filter;
                Idx = idx;
            }

            public SDataC Value
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

        public void AddBefore(LinkEnum idx, in SDataC v)
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
            readonly PoolArray_SDataC _filter;
            readonly int _count;
            int _idx;


            internal Enumerator(PoolArray_SDataC filter)
            {
                _filter = filter;
                _count = _filter.Count;
                _idx = -1;
                _filter.Lock = true;
            }

            public SDataC Current
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

            obj = TArray_SDataC.New(capacity);

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

            obj = TArray_SDataC.New(capacity);

            if (old1 != null)
            {
                old1.Release();
            }

            bDirty = true;
        }
    }


    public class TArray_SDataC
    {
        public static TArray_SDataC New(int capacity)
        {
            var obj = new TArray_SDataC();
            return obj;
        }

        private static TArray_SDataC InstanceFunc(object condition)
        {
            if (condition == null)
            {
                return new TArray_SDataC();
            }
            else
            {
                var args = condition is int ? (int) condition : 32;
                return new TArray_SDataC(args);
            }
        }

        public void Delete()
        {
        }

        public void Release()
        {
            Delete();
        }


        public SDataC[] Items;
        public int Count;

#if DEBUG
        private int uuid;
#endif

        protected TArray_SDataC() : this(32)
        {
        }

        protected TArray_SDataC(int capacity)
        {
            Items = new SDataC[capacity];
            Count = 0;
        }
    }
}