using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotkeyController : MonoBehaviour
{
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;
    private SpriteRenderer spireRenderer => GetComponent<SpriteRenderer>();
    private BlackHoleSkillController blackHoleSkillController;
    private Transform myEnemy;

    public void SetupHotkey(KeyCode _myHotKey,Transform _myEnemy, BlackHoleSkillController _blackHoleSkillController)
    {
        this.myHotkey = _myHotKey;
        this.blackHoleSkillController = _blackHoleSkillController;
        this.myEnemy = _myEnemy;

        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            //Debug.Log("Hotkey is " + myHotkey.ToString());
            blackHoleSkillController.AddEnemyToTargetList(myEnemy);
            myText.color = Color.clear;
            spireRenderer.color = Color.clear;
        }
    }
}
