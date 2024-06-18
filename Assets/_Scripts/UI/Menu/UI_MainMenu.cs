using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] string sceneName = "Game";
    // Start is called before the first frame update
    public void Continue()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
