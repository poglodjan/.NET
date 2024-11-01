using System;
using System.Collections;
using System.Collections.Generic;

namespace P3Z_24Z_Lab05;

public class MySortedLinkedList<T> : IMyCollection<T> where T : IComparable<T>
{
    private class Node
    {
        public T Value { get; set; }
        public Node? Next { get; set; }
        public Node(T value)
        {
            Value = value;
            Next = null;
        }
    }

    private Node? _head;
    private int _count; 

    public int Count => _count;

    public void Add(T item)
    {
        var newNode = new Node(item);

        if (_head == null || _head.Value.CompareTo(item) > 0)
        {
            newNode.Next = _head;
            _head = newNode;
        }
        else
        {
            Node current = _head;
            while (current.Next != null && current.Next.Value.CompareTo(item) < 0)
            {
                current = current.Next;
            }

            newNode.Next = current.Next;
            current.Next = newNode;
        }
        _count++;
    }

    public bool Contains(T item)
    {
        Node? current = _head;
        while (current != null)
        {
            if (current.Value.CompareTo(item) == 0)
                return true;
            current = current.Next;
        }
        return false;
    }

    public T PopFront()
    {
        if (_head == null)
            throw new IndexOutOfRangeException("The list is empty.");

        T value = _head.Value;
        _head = _head.Next;
        _count--;

        return value;
    }

    public IEnumerator<T> GetEnumerator()
    {
        Node? current = _head;
        while (current != null)
        {
            yield return current.Value;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}