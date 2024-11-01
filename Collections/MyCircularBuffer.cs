using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace P3Z_24Z_Lab05;

public interface IMyCollection<T> : IEnumerable<T>
    {
        int Count { get; }
        void Add(T item);
    }

public class MyCircularBuffer<T> : IMyCollection<T>
    {
        private readonly T[] _buffer;
        private int _head; //miejsce gdzie mozna dodac nowe dane
        private int _tail; // najstarszy element
        private int _count; // liczy ile jest elementow
        public int Count => _count;
        public bool IsFull
        {
            get { return _count == _buffer.Length; }
        }
        public bool IsEmpty
        {
            get { return Count == 0; }
        }

        public MyCircularBuffer(int capacity)
        {
            _buffer = new T[capacity];
            _head = 0;
            _tail = 0;
            _count = 0;
        }

        public void Add(T item)
        {
            _buffer[_head] = item;

            if (IsFull) // jeśli bufor jest pełny, przesuwamy ogon, aby nadpisać najstarszy element
            {
                _tail = (_tail + 1) % _buffer.Length;
            }
            else
            {
                _count++; // zwiększamy liczbę elementów tylko wtedy, gdy bufor nie jest pełny
            }

            _head = (_head + 1) % _buffer.Length; // przesuwamy wskaźnik głowy na następny element
        }

        public IEnumerable<T> GetItems()
        {
            for(int i=0; i<Count; i++)
            {
                yield return _buffer[(_tail + i) % _buffer.Length];
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            if (!IsFull)
            {
                for (int i = 0; i < _count; i++)
                {               
                    yield return _buffer[(_tail + i) % _buffer.Length];
                }
            }
            if (IsFull)
            {
                for (int i = 0; i < double.PositiveInfinity; i++)
                {
                    yield return _buffer[(_tail + i) % _buffer.Length];
                }
            }
        }
        


        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
