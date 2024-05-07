using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour
{
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;

    public void SetupHotkey(KeyCode _myHotKey)
    {
        myText = GetComponent<TextMeshProUGUI>();
        myText.text = myHotkey.ToString();

        this.myHotkey = _myHotKey;
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            Debug.Log("Hotkey is " + myHotkey.ToString());
        }
    }
}
