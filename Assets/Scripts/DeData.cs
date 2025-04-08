using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeData: MonoBehaviour
{
    private static DeData _instance;

    public static DeData Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public void SetEnterLevel(int chapter, int level)
    {
        this.enterChapter = chapter;
        this.enterLevel = level;
    }

    public string GetEnterLevelName()
    {
        return this.enterChapter + "_" + this.enterLevel;
    }

    public string GetCurrentLevelName()
    {
        return this.enterChapter + "-" + this.enterLevel;
    }

    public void Initialize()
    {
        LoadLevelData();
    }

    private void SaveLevelData()
    {
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string fullPath = Path.Combine(filePath, levelFileName);
        string jsonData = JsonUtility.ToJson(this.ownLevelSaveDataList);

        File.WriteAllText(fullPath, jsonData);
    }

    private void LoadLevelData()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = Application.persistentDataPath;  // Ensure it has a valid value
        }
        string fullPath = Path.Combine(filePath, levelFileName);
        if (!File.Exists(fullPath))
        {

            TextAsset textAsset = Resources.Load<TextAsset>(fileName);
            if (textAsset != null)
            {
                ownLevelSaveDataList = JsonUtility.FromJson<LevelSaveDataList>(textAsset.text);
            }
            else
            {
                Debug.LogError("Không tìm thấy file LevelGameData.json trong thư mục Resources!");
            }

            if (ownLevelSaveDataList == null || ownLevelSaveDataList.data == null)
            {
                ownLevelSaveDataList = new LevelSaveDataList { data = new List<LevelSaveData>() };
            }

            SaveLevelData();
            return;
        }

        string jsonText = File.ReadAllText(fullPath);
        Debug.Log("Dedata: " + jsonText);
        ownLevelSaveDataList = JsonUtility.FromJson<LevelSaveDataList>(jsonText);

        if (ownLevelSaveDataList == null || ownLevelSaveDataList.data == null)
        {
            ownLevelSaveDataList = new LevelSaveDataList { data = new List<LevelSaveData>() };
        }
    }


    //public LevelSaveData GetLevelData(int chapter, int level)
    //{
    //    foreach (var data in this.ownLevelSaveDataList.data)
    //    {
    //        if (data.Chapter == chapter && data.LevelID == level)
    //        {
    //            return data;
    //        }
    //    }
    //    return null;
    //}
    public LevelSaveData GetLevelData(int chapter, int level)
    {
        if (ownLevelSaveDataList == null)
        {
            return null;
        }
        if (ownLevelSaveDataList.data == null)
        {
            return null;
        }
        foreach (var data in ownLevelSaveDataList.data)
        {
            if (data.Chapter == chapter && data.LevelID == level)
            {
                return data;
            }
        }
        return null;
    }


    public void OpenLevelStartState(int chapter, int level)
    {
        LevelSaveData levelData = GetLevelData(chapter, level);
        if (levelData != null)
        {
            levelData.CanStart = true;
        }
    }

    public void SaveLevel()
    {
        LevelSaveData levelData = GetLevelData(this.enterChapter, this.enterLevel);
        if (levelData == null || levelData.Passed)
        {
            return;
        }

        levelData.Passed = true;

        if (this.enterChapter == 3 && this.enterLevel == 3)
        {
            SaveLevelData();
            return;
        }

        LevelSaveData nextLevel = GetLevelData(this.enterChapter, this.enterLevel + 1);
        if (nextLevel != null)
        {
            nextLevel.CanStart = true;
        }
        else
        {
            LevelSaveData firstLevelNextChapter = GetLevelData(this.enterChapter + 1, 0);
            if (firstLevelNextChapter != null)
            {
                firstLevelNextChapter.CanStart = true;
            }
        }

        SaveLevelData();
    }
    private int enterChapter;
    private int enterLevel;

    private static string fileName = "LevelGameData";
    private string filePath;
    private static readonly string levelFileName = fileName + ".txt";

    public DeData.LevelSaveDataList ownLevelSaveDataList;

    [Serializable]
    public class LevelSaveDataList
    {
        public List<LevelSaveData> data;
    }

    [Serializable]
    public class LevelSaveData
    {
        public int Chapter;
        public int LevelID;
        public bool Passed;
        public bool CanStart;
    }
}
