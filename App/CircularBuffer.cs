namespace App;

public class CircularBuffer<T>
{
    private T[] _buffer;
    private int _head;
    private int _tail;
    private int _capacity;

    public CircularBuffer(int capacity)
    {
        _capacity = capacity;
        _tail = 0;
        _head = 0;
        _buffer = new T[capacity];
    }

    public void Add(T item)
    {
        _buffer[_tail] = item;
        _tail = (_tail + 1) % _capacity;
    }

    public T Get()
    {
        var item = _buffer[_head];
        return item;
    }

    public void UpdateHead()
    {
        _head = (_head + 1) % _capacity;
    }
}