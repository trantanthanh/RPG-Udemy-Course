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

    private bool isAddedTolist = false;

    public void SetupHotkey(KeyCode _myHotKey,Transform _myEnemy, BlackHoleSkillController _blackHoleSkillController)
    {
        isAddedTolist = false;
        this.myHotkey = _myHotKey;
        this.blackHoleSkillController = _blackHoleSkillController;
        this.myEnemy = _myEnemy;

        myText = GetComponentInChildren<TextMeshProUGUI>();
        myText.text = myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey) && !isAddedTolist)
        {
            isAddedTolist = true;
            //Debug.Log("Hotkey is " + myHotkey.ToString());
            blackHoleSkillController.AddEnemyToTargetList(myEnemy);
            myText.color = Color.clear;
            spireRenderer.color = Color.clear;
        }
    }
}
