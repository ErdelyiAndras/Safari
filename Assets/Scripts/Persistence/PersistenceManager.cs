using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class PersistenceManager
{
    [System.Serializable]
    private class Data
    {
        public Difficulty difficulty;
        public TimeData timeData;
        public EconomyManagerData economyManagerData;
        public TouristManagerData touristManagerData;
        public GridData gridData;
    }

    private static readonly string saveFolderPath = Path.Combine(Application.dataPath, "Saves"); // Application.persistentDataPath;

    private static Data data = new Data();

    public static Difficulty Difficulty
    {
        get
        {
            return data.difficulty;
        }
        set
        {
            data.difficulty = value;
        }
    }

    public static TimeData TimeData
    {
        get
        {
            return data.timeData;
        }
        set
        {
            data.timeData = value;
        }
    }

    public static EconomyManagerData EconomyManagerData
    {
        get
        {
            return data.economyManagerData;
        }
        set
        {
            data.economyManagerData = value;
        }
    }

    public static TouristManagerData TouristManagerData
    {
        get
        {
            return data.touristManagerData;
        }
        set
        {
            data.touristManagerData = value;
        }
    }

    public static GridData GridData
    {
        get
        {
            return data.gridData;
        }
        set
        {
            data.gridData = value;
        }
    }

    public static void Save(string fileName)
    {
        string filePath = Path.Combine(saveFolderPath, fileName);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
    }

    public static void Load(string fileName)
    {
        string filePath = Path.Combine(saveFolderPath, fileName);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(json);
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
        }
    }
}
