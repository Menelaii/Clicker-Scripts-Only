
public class PoolObjectContainer<T>
{
    public PoolObjectContainer(T item)
    {
        Item = item;
        Used = false;
    }

    public T Item { get; private set; }
    public bool Used { get; private set; }

    public void Consume()
    {
        Used = true;
    }

    public void Release()
    {
        Used = false;
    }
}
