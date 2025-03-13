using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

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
    }
    public void LoadGame()
    {
    }
    public void SaveGame()
    {

    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
