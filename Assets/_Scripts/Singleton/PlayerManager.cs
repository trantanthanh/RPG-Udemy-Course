using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager Instance;
    public Player player;
    public UI_Ingame uiIngame;

    public int currentCurrency;
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    public bool HaveEnoughMoney(int _price)
    {
        if (_price > currentCurrency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        currentCurrency -= _price;
        return true;
    }

    public int GetCurrency() => currentCurrency;

    public void LoadData(GameData _data)
    {
        this.currentCurrency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currentCurrency;
    }
}
