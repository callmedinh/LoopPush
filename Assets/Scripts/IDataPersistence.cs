using UnityEngine;

public interface IDataPersistence
{
    void LoadGame(LevelData data);
    void SaveGame(ref LevelData data);
}
