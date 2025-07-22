using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeData : MonoBehaviour
{
    private static readonly string fileName = "LevelGameData";
    private static readonly string levelFileName = fileName + ".txt";

    public LevelSaveDataList ownLevelSaveDataList;
    private int enterChapter;
    private int enterLevel;
    private string filePath;

    public static DeData Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public void SetEnterLevel(int chapter, int level)
    {
        enterChapter = chapter;
        enterLevel = level;
    }

    public string GetEnterLevelName()
    {
        return enterChapter + "_" + enterLevel;
    }

    public string GetCurrentLevelName()
    {
        return enterChapter + "-" + enterLevel;
    }

    public void Initialize()
    {
        LoadLevelData();
    }

    private void SaveLevelData()
    {
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

        var fullPath = Path.Combine(filePath, levelFileName);
        var jsonData = JsonUtility.ToJson(ownLevelSaveDataList);

        File.WriteAllText(fullPath, jsonData);
    }

    private void LoadLevelData()
    {
        if (string.IsNullOrEmpty(filePath)) filePath = Application.persistentDataPath; // Ensure it has a valid value
        var fullPath = Path.Combine(filePath, levelFileName);
        if (!File.Exists(fullPath))
        {
            var textAsset = Resources.Load<TextAsset>(fileName);
            if (textAsset != null)
                ownLevelSaveDataList = JsonUtility.FromJson<LevelSaveDataList>(textAsset.text);
            else
                Debug.LogError("Không tìm thấy file LevelGameData.json trong thư mục Resources!");

            if (ownLevelSaveDataList == null || ownLevelSaveDataList.data == null)
                ownLevelSaveDataList = new LevelSaveDataList { data = new List<LevelSaveData>() };

            SaveLevelData();
            return;
        }

        var jsonText = File.ReadAllText(fullPath);
        Debug.Log("Dedata: " + jsonText);
        ownLevelSaveDataList = JsonUtility.FromJson<LevelSaveDataList>(jsonText);

        if (ownLevelSaveDataList == null || ownLevelSaveDataList.data == null)
            ownLevelSaveDataList = new LevelSaveDataList { data = new List<LevelSaveData>() };
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
        if (ownLevelSaveDataList == null) return null;
        if (ownLevelSaveDataList.data == null) return null;
        foreach (var data in ownLevelSaveDataList.data)
            if (data.Chapter == chapter && data.LevelID == level)
                return data;

        return null;
    }


    public void OpenLevelStartState(int chapter, int level)
    {
        var levelData = GetLevelData(chapter, level);
        if (levelData != null) levelData.CanStart = true;
    }

    public void SaveLevel()
    {
        var levelData = GetLevelData(enterChapter, enterLevel);
        if (levelData == null || levelData.Passed) return;

        levelData.Passed = true;

        if (enterChapter == 3 && enterLevel == 3)
        {
            SaveLevelData();
            return;
        }

        var nextLevel = GetLevelData(enterChapter, enterLevel + 1);
        if (nextLevel != null)
        {
            nextLevel.CanStart = true;
        }
        else
        {
            var firstLevelNextChapter = GetLevelData(enterChapter + 1, 0);
            if (firstLevelNextChapter != null) firstLevelNextChapter.CanStart = true;
        }

        SaveLevelData();
    }

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