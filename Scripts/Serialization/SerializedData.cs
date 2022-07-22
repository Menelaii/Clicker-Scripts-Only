using System;
using UnityEngine;

[Serializable]
public class SerializedData
{
	public string Data;
	public string TypeName;

	private Type Type => Type.GetType(TypeName);

	public static SerializedData Serialize(object obj)
	{
		if (obj == null)
			return null;

		var result = new SerializedData()
		{
			TypeName = obj.GetType().FullName,
			Data = JsonUtility.ToJson(obj)
		};

		return result;
	}

	public static object Deserialize(SerializedData sd)
	{
		return JsonUtility.FromJson(sd.Data, sd.Type);
	}
}
