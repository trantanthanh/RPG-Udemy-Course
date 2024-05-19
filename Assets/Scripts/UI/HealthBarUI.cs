using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        entity.onFlipped += FlipUI;
    }
    private void FlipUI()
    {
        transform.Rotate(0, 180, 0);
    }
}
