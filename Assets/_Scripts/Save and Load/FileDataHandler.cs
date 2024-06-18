using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool isEncrypt = false;
    private string encryptCode = "thanhDev";

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _isEncrypt)
    {
        this.dataDirPath = _dataDirPath;
        this.dataFileName = _dataFileName;
        this.isEncrypt = _isEncrypt;
    }

    public void Save(GameData _data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataJson = JsonUtility.ToJson(_data, true);

            if (isEncrypt)
            {
                dataJson = EncryptDecrypt(dataJson);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataJson);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file : " + fullPath + "\n" + e);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataJson = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataJson = reader.ReadToEnd();
                    }
                }

                if (isEncrypt)
                {
                    dataJson = EncryptDecrypt(dataJson);
                }

                loadData = JsonUtility.FromJson<GameData>(dataJson);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to Load data from : " + fullPath + "\n" + e);
            }
        }
        return loadData;
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (File.Exists(fullPath))
            File.Delete(fullPath);
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";

        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] ^ encryptCode[i % encryptCode.Length]);
        }

        return modifiedData;

    }
}
