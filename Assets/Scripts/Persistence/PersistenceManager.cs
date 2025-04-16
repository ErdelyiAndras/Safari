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
        public AnimalManagerData animalManagerData;
        public TouristManagerData touristManagerData;
        public PlacementManagerData placementManagerData;
    }

    private static readonly string saveFolderPath = Application.persistentDataPath; // Path.Combine(Application.dataPath, "Saves"); 

    private static Data data = new Data();

    public static bool MainMenuLoad { get; set; } = false;

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

    public static AnimalManagerData AnimalManagerData
    {
        get
        {
            return data.animalManagerData;
        }
        set
        {
            data.animalManagerData = value;
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

    public static PlacementManagerData PlacementManagerData
    {
        get
        {
            return data.placementManagerData;
        }
        set
        {
            data.placementManagerData = value;
        }
    }

    public static void Save(string fileName)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath(fileName), json);
    }

    public static void Load(string fileName)
    {
        if (SaveExists(fileName))
        {
            string json = File.ReadAllText(FilePath(fileName));
            data = JsonUtility.FromJson<Data>(json);
        }
        else
        {
            Debug.LogError($"File not found: {FilePath(fileName)}");
        }
    }

    public static bool SaveExists(string fileName)
    {
        return File.Exists(FilePath(fileName));
    }

    private static string FilePath(string fileName)
    {
        return Path.Combine(saveFolderPath, fileName);
    }
}
