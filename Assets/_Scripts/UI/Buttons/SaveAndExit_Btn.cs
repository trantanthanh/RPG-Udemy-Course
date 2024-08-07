using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class SaveAndExit_Btn : BaseButton
{
    protected override void Start()
    {
        base.Start();
#if UNITY_WEBGL
        button.GetComponentInChildren<TextMeshProUGUI>().text = "Save game";
#endif
    }
    protected override void OnClick()
    {
        SaveManager.Instance.SaveGame();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#elif !UNITY_WEBGL
        Application.Quit();
#endif
    }
}
