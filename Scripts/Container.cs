using UnityEngine;

public abstract class Container<T> : ScriptableObject
{
    public abstract T Item { get; }

    public T GetClone()
    {
        return CloneUtil.CreateClone(Item);
    }
}
