using System;

public class stack<T>
{
        private T[] _items = new T[100];
        public int count {get; private set;}
        public void push(T item) => _items[count++] = item; // inserts item on top
        public T pop() => _items[--count];
}

public static T Max<T>(T value, params T[] values)
where T : IComparable<T>
{
    var max = value;
    foreach(var t in values)
    {
        if(max.CompareTo(t) < 0)
        {
            max = t;
        }
    }
    return max;
}

public class CircularBuffer<T>
{
    private readonly T[] _buffer;
    private int _head;   // Indeks, pod którym zostanie zapisany następny element
    private int _tail;   // Indeks, pod którym znajduje się najstarszy element
    private int _count;  // Aktualna liczba elementów w buforze

    public int Capacity { get; }
    public int Count => _count;  // Liczba elementów aktualnie w buforze
    public bool IsFull => _count == Capacity;
    public bool IsEmpty => _count == 0;

    // Konstruktor inicjalizujący bufor o podanej pojemności
    public CircularBuffer(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));
        
        Capacity = capacity;
        _buffer = new T[capacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    // Metoda dodająca element do bufora
    public void Enqueue(T item)
    {
        _buffer[_head] = item;
        _head = (_head + 1) % Capacity; // Przejście do następnej pozycji (cyklicznie)

        if (IsFull)
        {
            _tail = (_tail + 1) % Capacity; // Przesunięcie ogona, jeśli bufor jest pełny
        }
        else
        {
            _count++;
        }
    }

    // Metoda pobierająca element z bufora
    public T Dequeue()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Buffer is empty.");

        var item = _buffer[_tail];
        _tail = (_tail + 1) % Capacity; // Przesunięcie ogona na następny element
        _count--;
        return item;
    }

    // Metoda podglądająca najstarszy element bez usuwania
    public T Peek()
    {
        if (IsEmpty)
            throw new InvalidOperationException("Buffer is empty.");

        return _buffer[_tail];
    }
}


stack<int> intStack = new stack<int>();
for(int i=0; i<10; i++)
{
    intStack.push(i);
} 

while(intStack.count > 0)
{
    int item = intStack.pop();
    Console.Write(item);
}
