using System.Collections.Generic;

public static class AbstractObjectsSerializer
{
    public static List<SerializedData> Serialize<T>(List<T> objects)
    {
        if (objects == null)
            return null;

        List<SerializedData> bodyPartsSerialized = new List<SerializedData>();
        foreach (var abstractClass in objects)
        {
            bodyPartsSerialized.Add(SerializedData.Serialize(abstractClass));
        }

        return bodyPartsSerialized;
    }

    public static List<T> Deserialize<T>(List<SerializedData> serializedObjects)
    {
        if (serializedObjects == null)
            return null;

        List<T> objects = new List<T>();
        foreach (var serialized in serializedObjects)
        {
            var abstractClass = (T)SerializedData.Deserialize(serialized);
            objects.Add(abstractClass);
        }

        return objects;
    }
}
