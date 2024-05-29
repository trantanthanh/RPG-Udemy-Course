using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike", menuName = "Data/Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] GameObject thunderstrikePrefab;
    public override void ExecuteEffect(Transform _target)
    {
        GameObject newThunderStrike = Instantiate(thunderstrikePrefab, _target.position, Quaternion.identity);

        //TODO: Setup new thunder strike
    }
}
