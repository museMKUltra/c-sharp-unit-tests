using System;
using System.Collections.Generic;

namespace ClassLibrary1.Fundamentals
{
    public class Stack<T>
    {
        private readonly List<T> _list = new List<T>();
        public int Count => _list.Count;

        public void Push(T item)
        {
            if (item == null)
                throw new ArgumentNullException();

            _list.Add(item);
        }

        public T Pop()
        {
            if (_list.Count == 0)
                throw new InvalidOperationException();

            var index = _list.Count - 1;
            var pop = _list[index];
            _list.RemoveAt(index);
            return pop;
        }

        public T Peek()
        {
            if (_list.Count == 0)
                throw new InvalidOperationException();

            return _list[_list.Count - 1];
        }
    }
}