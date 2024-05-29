using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Item Effect", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] GameObject thunderstrikePrefab;
    public override void ExecuteEffect()
    {
        GameObject newThunderStrike = Instantiate(thunderstrikePrefab);

        //TODO: Setup new thunder strike
    }
}
