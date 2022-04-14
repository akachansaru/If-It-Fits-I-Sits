using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Add this class to a Game Object to save and load data specified in SaveValues.cs
/// </summary>
/// <returns></returns>
public class GlobalControl : MonoBehaviour
{

    public static GlobalControl Instance;

    public SaveValues savedValues;

    string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Creating new GlobalControl");
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Updating GlobalControl");
            Destroy(gameObject);
        }
        savePath = Application.persistentDataPath + "/saveValues.sheep";
        Load();
    }

    public bool IsNewGame()
    {
        return savedValues.SaveFile == "";
    }

    /// <summary>
    ///  Initialize save values when starting a new game.
    /// </summary>
    public void CreateNewGame()
    {
        savedValues.SaveFile = "";
        savedValues.MusicMuted = false;
        savedValues.MusicVolume = 1f; // Full volume

        Debug.Log("New game");
    }

    public void Save()
    {
        savedValues.SaveFile = savePath;
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(savePath);
        bf.Serialize(file, savedValues);
        file.Close();
        Debug.Log("Saved to " + savePath);
    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            savedValues = (SaveValues)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            CreateNewGame();
        }
    }
}
