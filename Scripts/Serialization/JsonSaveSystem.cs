using System.IO;
using UnityEngine;

public class JsonSaveSystem
{
    private readonly string _filePath;

    public JsonSaveSystem()
    {
        _filePath = Application.persistentDataPath + "/Save.json";
    }

    public void Save(PlayerProgress data)
    {
        var json = JsonUtility.ToJson(data);
        using (var writer = new StreamWriter(_filePath))
        {
            writer.WriteLine(json);
        }
    }

    public PlayerProgress Load()
    {
        string json = "";
        using (var reader = new StreamReader(_filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                json += line;
            }
        }

        if (string.IsNullOrEmpty(json))
        {
            return new PlayerProgress();
        }

        return JsonUtility.FromJson<PlayerProgress>(json);
    }

    public bool IsSaveFileExist()
    {
        return File.Exists(_filePath);
    }
}