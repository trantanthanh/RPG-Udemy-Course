using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect_SO : ScriptableObject
{
    public virtual void ExecuteEffect(Transform _target)
    {
        Debug.Log("Execute Effect");
    }
}
