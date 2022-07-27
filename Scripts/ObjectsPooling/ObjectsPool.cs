using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool<T> where T : MonoBehaviour
{
	private List<PoolObjectContainer<T>> _containers;
	private Dictionary<T, PoolObjectContainer<T>> _lookup;
	private Func<T> _factoryFunc;
	private int lastIndex = 0;

	public ObjectsPool(Func<T> factoryFunc, int initialSize)
	{
		_factoryFunc = factoryFunc;

		_containers = new List<PoolObjectContainer<T>>(initialSize);
		_lookup = new Dictionary<T, PoolObjectContainer<T>>(initialSize);

		WarmUp(initialSize);
	}

	public T GetItem()
    {
        PoolObjectContainer<T> container = GetFreeContainer();
        container.Consume();

        _lookup.Add(container.Item, container);

        return container.Item;
    }

	public void ReleaseItem(T item)
	{
		if (_lookup.ContainsKey(item))
		{
			var container = _lookup[item];
			container.Release();
			_lookup.Remove(item);
		}
		else
		{
			Debug.LogWarning("This object pool does not contain the item provided: " + item);
		}
	}

	private void WarmUp(int capacity)
	{
		for (int i = 0; i < capacity; i++)
		{
			CreateContainer();
		}
	}

	private PoolObjectContainer<T> CreateContainer()
	{
		var container = new PoolObjectContainer<T>(_factoryFunc());
		_containers.Add(container);
		return container;
	}

	private PoolObjectContainer<T> GetFreeContainer()
    {
		PoolObjectContainer<T> container = null;
		for (int i = 0; i < _containers.Count; i++)
        {
            lastIndex++;
            if (lastIndex > _containers.Count - 1)
                lastIndex = 0;

            if (_containers[lastIndex].Used)
            {
                continue;
            }
            else
            {
                container = _containers[lastIndex];
                break;
            }
        }

		if (container == null)
		{
			container = CreateContainer();
		}

		return container;
    }
}
