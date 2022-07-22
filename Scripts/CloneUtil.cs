
public static class CloneUtil
{
    public static T CreateClone<T>(T obj)
    {
        var data = SerializedData.Serialize(obj);

        return (T)SerializedData.Deserialize(data);
    }
}
