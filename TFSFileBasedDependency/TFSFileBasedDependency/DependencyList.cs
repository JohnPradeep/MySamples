using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DependentTFSTracking
{
    public class DependencyList<T> : IList<T>
    {
        private readonly IList<T> m_dependencyList = new List<T>();

        private int lastUsedElementIndex;

        public T MoveNext()
        {
            int temp = lastUsedElementIndex;
            lastUsedElementIndex = lastUsedElementIndex + 1 >= m_dependencyList.Count ? 0 : lastUsedElementIndex + 1;
            return m_dependencyList[temp];
        }

        public T MovePrevious()
        {
            int temp = lastUsedElementIndex;
            lastUsedElementIndex = lastUsedElementIndex - 1 < 0 ? m_dependencyList.Count - 1 : lastUsedElementIndex - 1;
            return m_dependencyList[temp];
        }

        public T Current
        {
            get
            {
                return m_dependencyList.Count == 0 ? default(T) : m_dependencyList[lastUsedElementIndex];
            }
        }

        public void Reset()
        {
            lastUsedElementIndex = 0;
        }

        public DependencyList() { }

        public DependencyList(int StartingIterableIndex = 0)
        {
            lastUsedElementIndex = StartingIterableIndex;
        }

        public DependencyList(IEnumerable<T> source)
        {
            m_dependencyList = source.ToDependencyListNavigator();
        }

        public DependencyList<T> ConvertToDependencyListNavigator(IEnumerable<T> collection, int startingIterableIndex)
        {
            DependencyList<T> iterableCollection = new DependencyList<T>(startingIterableIndex);
            foreach (var item in collection)
            {
                iterableCollection.Add(item);
            }
            return iterableCollection;
        }

        public T this[int index]
        {
            get { return m_dependencyList[index]; }
            set { m_dependencyList[index] = value; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_dependencyList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return m_dependencyList.Count; }
        }

        public bool IsReadOnly
        {
            get { return m_dependencyList.IsReadOnly; }
        }

        public void Add(T item)
        {
            m_dependencyList.Add(item);
        }

        public void Clear()
        {
            m_dependencyList.Clear();
        }

        public bool Contains(T item)
        {
            return m_dependencyList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_dependencyList.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return m_dependencyList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_dependencyList.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return m_dependencyList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            m_dependencyList.RemoveAt(index);
        }

    }
}
