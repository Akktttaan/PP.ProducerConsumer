namespace App;

public class ProducerConsumer<T>
{
    private SemaphoreSlim _full;
    private SemaphoreSlim _empty;
    private SemaphoreSlim _binary;
    private CircularBuffer<T> _buffer; 
    
    public ProducerConsumer(int capacity)
    {
        _buffer = new CircularBuffer<T>(capacity);
        _binary = new SemaphoreSlim(1, 1);
        _full = new SemaphoreSlim(0, capacity);
        _empty = new SemaphoreSlim(capacity, capacity);
    }

    public void Produce(T element)
    {
        _empty.Wait();
        _binary.Wait();
        
        _buffer.Add(element);

        _binary.Release();
        _full.Release();
    }

    public T Consume(Predicate<T> predicate)
    {
        _full.Wait();
        _binary.Wait();

        var element = _buffer.Get();

        var flag = predicate.Invoke(element);
        if (flag)
        {
            _empty.Release();
            _buffer.UpdateHead();
        }
        else
        {
            element = Activator.CreateInstance<T>();
            _full.Release();
        }
        _binary.Release();
        return element;
    }
}