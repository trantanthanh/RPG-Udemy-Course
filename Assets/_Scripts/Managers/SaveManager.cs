using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    [SerializeField] string fileSaveName;
#if PLATFORM_WEBGL
    [SerializeField] string filePath = "idbfs/thanhdev123987465/rpg-course-game";
#endif
    [SerializeField] bool encryptData;

    [Header("Skill Tree")]
    [SerializeField] GameObject skillTreeObj;

    private GameData gameData;
    List<ISaveManager> saveManagers;
    private FileDataHandler fileDataHandler;

    private bool isLoaded = false;
    public bool IsLoaded => isLoaded;

    [ContextMenu("Delete save file")]
    public void DeleteSavedData()
    {
        ConstructFileDataHandler();
        fileDataHandler.Delete();
    }

    private void Awake()
    {
        isLoaded = false;
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
        ConstructFileDataHandler();
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    private void ConstructFileDataHandler()
    {
#if UNITY_EDITOR
        fileDataHandler = new FileDataHandler("D:\\Unity\\RPG-Udemy-Course", fileSaveName, encryptData);
#elif PLATFORM_WEBGL
        fileDataHandler = new FileDataHandler(filePath, fileSaveName, encryptData);
#else
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileSaveName, encryptData);
#endif
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
        isLoaded = true;
    }

    public void SaveGame()
    {
        //Debug.Log("Game was saved");
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
        //Debug.Log("Save currency " + gameData.currency);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> _saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();

        if (skillTreeObj != null)
        {

            ISaveManager[] _skillTreeSaveManager = skillTreeObj.GetComponentsInChildren<ISaveManager>(true);//Find all inactive object
            IEnumerable<ISaveManager> _allSaveManagers = _saveManagers.Concat(_skillTreeSaveManager);
            return new List<ISaveManager>(_allSaveManagers);
        }

        return new List<ISaveManager>(_saveManagers);
    }

    public bool HasSaveData()
    {
        ConstructFileDataHandler();
        if (fileDataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }
}
