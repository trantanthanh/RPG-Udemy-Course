using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] string sceneName = "Game";
    [SerializeField] GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    // Start is called before the first frame update
    private void Start()
    {
        if (!SaveManager.Instance.HasSaveData())
        {
            continueButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(true);
        }
        fadeScreen.FadeIn();
    }

    public void Continue()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return _delay.WaitForSeconds();
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
