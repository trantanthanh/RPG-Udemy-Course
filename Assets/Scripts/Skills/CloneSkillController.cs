using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] Vector3 offsetPos;
    public void SetupClone(Transform _newTransform)
    {
        transform.position = _newTransform.position + offsetPos;
    }
}
