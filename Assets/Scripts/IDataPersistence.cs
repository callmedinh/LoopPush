using UnityEngine;

public interface IDataPersistence
{
    void LoadGame(LevelSaveData data);
    void SaveGame(ref LevelSaveData data);
}
