using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AMBehaviorSystem.Core.Utilities
{
    [Serializable]
    public class ObservableList<T> : IList<T>, IReadOnlyList<T>
    {
        [SerializeReference] protected List<T> Items = new();
        
        public event Action<T> OnAdded;
        public event Action<T> OnRemoved;
        public event Action OnCleared;

        public int Count => Items.Count;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }

        public void Add(T item)
        {
            Items.Add(item);
            OnAdded?.Invoke(item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
                Add(item);
        }

        public void Insert(int index, T item)
        {
            Items.Insert(index, item);
            OnAdded?.Invoke(item);
        }

        public bool Remove(T item)
        {
            bool result = Items.Remove(item);
            if (result) OnRemoved?.Invoke(item);
            return result;
        }

        public void RemoveAt(int index)
        {
            T item = Items[index];
            Items.RemoveAt(index);
            OnRemoved?.Invoke(item);
        }

        public int RemoveAll(Predicate<T> match)
        {
            int count = 0;
            for (int i = Items.Count - 1; i >= 0; i--)
            {
                if (!match(Items[i])) continue;
                T item = Items[i];
                Items.RemoveAt(i);
                OnRemoved?.Invoke(item);
                count++;
            }
            return count;
        }

        public void Clear()
        {
            Items.Clear();
            OnCleared?.Invoke();
        }

        public bool Contains(T item) => Items.Contains(item);
        public int IndexOf(T item) => Items.IndexOf(item);
        public int FindIndex(Predicate<T> match) => Items.FindIndex(match);
        public T Find(Predicate<T> match) => Items.Find(match);
        public List<T> FindAll(Predicate<T> match) => Items.FindAll(match);
        public bool Exists(Predicate<T> match) => Items.Exists(match);
        public T First() => Items[0];
        public T Last() => Items[^1];
        public void Sort(Comparison<T> comparison) => Items.Sort(comparison);
        public void Sort(IComparer<T> comparer) => Items.Sort(comparer);
        public void Reverse() => Items.Reverse();
        public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
        public T[] ToArray() => Items.ToArray();
        public List<T> ToList() => new(Items);
        public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();

        public static implicit operator List<T>(ObservableList<T> observableList) => observableList.Items;
    }
}
