using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [Space]
    [SerializeField] bool canAtack;
    public void CreateClone(Transform _newTransform, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_newTransform, cloneDuration, canAtack, _offset);
    }

}
