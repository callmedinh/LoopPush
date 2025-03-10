using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    LevelData levelData;
    public static DataPersistenceManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one DataPersistenceManager");
        }
        instance = this;
    }
    public void NewGame()
    {
        this.levelData = new LevelData();
    }
    public void LoadGame()
    {
        if (this.levelData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");
            NewGame();
        }
    }
    public void SaveGame()
    {

    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
