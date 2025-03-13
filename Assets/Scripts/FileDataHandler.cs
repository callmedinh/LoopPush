using System;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFilePath = "";

    public FileDataHandler(string dataDirPath, string dataFilePath)
    {
        this.dataDirPath = dataDirPath;
        this.dataFilePath = dataFilePath;
    }
    public LevelSaveData Load()
    {

    }
    public void Save(LevelSaveData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFilePath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
