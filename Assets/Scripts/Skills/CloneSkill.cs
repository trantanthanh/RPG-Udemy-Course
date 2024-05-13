using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    [Space]
    [SerializeField] bool canAtack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;
    [SerializeField] private bool canDuplicateClone;
    private int cloneDuplicateFacingDir = -1;
    [SerializeField] [Range(0,100)] private int percentToDuplicateClone;
    [SerializeField] private float offSetCloneCounterAttack = 1.5f;
    [SerializeField] private float timeDelayCreateCloneCounterAttack = 0.5f;
    public void CreateClone(Transform _newTransform, Vector3 _offset, Transform _targetToFacing = null, bool _canDuplicateClone = false, int _percentToDuplicateClone = 0)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_newTransform, cloneDuration, canAtack, _offset, _targetToFacing, _canDuplicateClone, _percentToDuplicateClone);
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
        {
            CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
        {
            //StartCoroutine(CreateCloneWithDelay(timeDelayCreateCloneCounterAttack, _enemyTransform));
            CreateCloneCanDuplicate(_enemyTransform);
        }
    }

    public void CreateCloneCanDuplicate(Transform _enemyTransform)
    {
        if (canDuplicateClone)
        {
            StartCoroutine(CreateCloneWithDelay(timeDelayCreateCloneCounterAttack, _enemyTransform, canDuplicateClone, percentToDuplicateClone));
        }
    }

    IEnumerator CreateCloneWithDelay(float _seconds, Transform _enemyTransform, bool _canDuplicateClone = false, int _percentToDuplicateClone = 0)
    {
        yield return new WaitForSeconds(_seconds);
        cloneDuplicateFacingDir = -cloneDuplicateFacingDir;
        CreateClone(_enemyTransform, new Vector3(offSetCloneCounterAttack * (canDuplicateClone ? cloneDuplicateFacingDir : player.facingDir), 0, 0), _enemyTransform, _canDuplicateClone, _percentToDuplicateClone);
    }
}
