using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager Instance;
    public Player player;
    public UI_Ingame uiIngame;
    public Menu.UI ui;

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

    public void UpdateCurrency(int _amountChange)
    {
        currentCurrency += _amountChange;
        if (currentCurrency <= 0)
        {
            currentCurrency = 0;
        }
    }

    public int GetCurrency() => currentCurrency;

    public void LoadData(GameData _data)
    {
        Debug.Log("PlayerManager load data _data.currency " + _data.currency);
        this.currentCurrency = _data.currency;
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = this.currentCurrency;
    }
}
