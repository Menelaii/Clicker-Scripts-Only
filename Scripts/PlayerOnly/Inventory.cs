using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Inventory : ISerializationCallbackReceiver
{
    [SerializeField] [HideInInspector]
    private List<SerializedData> _bodyPartsSerialized;
    [SerializeField] [HideInInspector]
    private int _size;

    private List<BodyPart> _bodyParts;

    public event Action<bool> ItemsCountChanged; 

    public Inventory(int size)
    {
        _bodyParts = new List<BodyPart>(size);
        _size = size;
    }

    public IReadOnlyList<BodyPart> BodyParts => _bodyParts;
    public bool IsFull => _bodyParts.Count >= _size;

    public void Add(BodyPart bodyPart)
    {
        if (IsFull)
            throw new InvalidOperationException();
            
        _bodyParts.Add(bodyPart);
        ItemsCountChanged?.Invoke(IsFull);
    }

    public void Remove(BodyPart bodyPart)
    {
        _bodyParts.Remove(bodyPart);
        ItemsCountChanged?.Invoke(IsFull);
    }

    public void OnBeforeSerialize()
    {
        _bodyPartsSerialized = AbstractObjectsSerializer.Serialize(_bodyParts);
    }

    public void OnAfterDeserialize()
    {
        _bodyParts = AbstractObjectsSerializer.Deserialize<BodyPart>(_bodyPartsSerialized);
    }
}
