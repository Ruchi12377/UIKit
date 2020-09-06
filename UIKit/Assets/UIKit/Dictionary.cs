using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace UIKit
{
    public sealed class Dictionary<TKey, TValue> where TKey : new() where TValue : new()
    {
        private DictionaryData<TKey, TValue> DataList = new DictionaryData<TKey, TValue>();
        public int Count { get { return DataList.Keys.Count; } }
        public List<TKey> Keys { get { return DataList.Keys; } }
        public List<TValue> Values { get { return DataList.Values; } }

        public void Add(TKey key, TValue value)
        {
            if (key == null) throw new ArgumentNullException();
            if (ContainsKey(key)) throw new ArgumentException();
            DataList.Keys.Add(key);
            DataList.Values.Add(value);
        }

        public void Clear()
        {
            DataList.Keys.Clear();
            DataList.Values.Clear();
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                try
                {
                    throw new ArgumentNullException();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                return false;
            }

            try
            {
                foreach (TKey _ in DataList.Keys) if ((dynamic)_ == (dynamic)key) return true;
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }

            return false;
        }

        public bool ContainsValue(TValue value)
        {
            try
            {
                foreach (TValue _ in DataList.Values) if ((dynamic)_ == (dynamic)value) return true;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            return false;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                try
                {
                    throw new ArgumentNullException();
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
                return false;
            }

            if (ContainsKey(key) == false) throw new ArgumentException();
            foreach (var _ in DataList.Keys.Select((Value, Index) => new { Value, Index }))
            {
                if ((dynamic)_.Value == (dynamic)key)
                {
                    DataList.Keys.Remove(_.Value);
                    DataList.Values.RemoveAt(_.Index);
                    return true;
                }
            }
            return false;
        }

        public bool TryAdd(TKey key, TValue value)
        {
            if (key == null)
            {
                try
                {
                    throw new ArgumentNullException();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                return false;
            }

            if (ContainsKey(key) == false)
            {
                DataList.Keys.Add(key);
                DataList.Values.Add(value);
                return true;
            }
            return false;
        }

        public bool TryRemove(TKey key)
        {
            if (key == null)
            {
                try
                {
                    throw new ArgumentNullException();
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
                return false;
            }

            if (ContainsKey(key))
            {
                foreach (var _ in DataList.Keys.Select((Value, Index) => new { Value, Index }))
                {
                    if ((dynamic)_.Value == (dynamic)key)
                    {
                        DataList.Keys.Remove(_.Value);
                        DataList.Values.RemoveAt(_.Index);
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public struct DictionaryData<DKey, DValue>
    {
        public List<DKey> Keys;
        public List<DValue> Values;

        public DictionaryData(List<DKey> Keys, List<DValue> Values)
        {
            this.Keys = Keys;
            this.Values = Values;
        }
    }
}
