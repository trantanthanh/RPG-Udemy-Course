using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] string fileSaveName;

    [Header("Skill Tree")]
    [SerializeField] GameObject skillTreeObj;

    private GameData gameData;
    List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    [ContextMenu("Delete save file")]
    private void DeleteSavedData()
    {
#if UNITY_EDITOR
        fileDataHandler = new FileDataHandler("D:\\Unity\\RPG-Udemy-Course", fileSaveName);
#else
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSaveName);
#endif
        fileDataHandler.Delete();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        fileDataHandler = new FileDataHandler("D:\\Unity\\RPG-Udemy-Course", fileSaveName);
#else
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSaveName);
#endif
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No data saved found");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("Load currency " + gameData.currency);
    }

    public void SaveGame()
    {
        Debug.Log("Game was saved");
        Debug.Log("saveManagers.Count " + saveManagers.Count);
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
        Debug.Log("Save currency " + gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> _saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        ISaveManager[] _skillTreeSaveManager = skillTreeObj.GetComponentsInChildren<ISaveManager>(true);//Find all inactive object

        IEnumerable<ISaveManager> _allSaveManagers = _saveManagers.Concat(_skillTreeSaveManager);

        return new List<ISaveManager>(_allSaveManagers);
    }
}
