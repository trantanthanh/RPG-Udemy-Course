using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckPointId = "";
    public List<string> equipmentId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;
        

    public GameData()
    {
        this.currency = 0;
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        checkpoints = new SerializableDictionary<string, bool>();
        equipmentId = new List<string>();
    }
}
