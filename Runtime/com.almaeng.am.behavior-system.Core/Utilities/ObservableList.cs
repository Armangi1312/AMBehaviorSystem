using System;
using System.Collections.Generic;

namespace AMBehaviorSystem.Core.Utilities
{
    [Serializable]
    public class ObservableList<T> : List<T>
    {
        public event Action<T> OnAdded;
        public event Action<T> OnRemoved;
        public event Action OnCleared;

        public new void Add(T item)
        {
            base.Add(item);
            OnAdded?.Invoke(item);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public new bool Remove(T item)
        {
            var result = base.Remove(item);
            if (result) OnRemoved?.Invoke(item);
            return result;
        }

        public new T RemoveAt(int index)
        {
            var item = this[index];
            base.RemoveAt(index);
            OnRemoved?.Invoke(item);
            return item;
        }

        public new void Clear()
        {
            base.Clear();
            OnCleared?.Invoke();
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            OnAdded?.Invoke(item);
        }
    }
}