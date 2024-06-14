using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] UI_SkillTreeSlot timeMirageUnlockButton;
    public bool mirageUnlocked { get; private set; }
    [Range(0.1f, 1f)]
    [SerializeField] float mirageDamagePercent;
    [SerializeField] GameObject clonePrefab;
    [SerializeField] float cloneDuration;
    bool canAtack = true;

    [Header("Clone features")]
    [SerializeField] UI_SkillTreeSlot blackHoleUnlockButton;
    public bool blackHoleUnlocked { get; private set; }
    [Space]
    [SerializeField] UI_SkillTreeSlot agressiveUnlockButton;
    [Range(0.5f, 1f)]
    [SerializeField] float mirageDamageAgreesivePercent;//80% damage, can hit effect
    public bool agressiveMirageUnlocked { get; private set; }

    [Space]
    [SerializeField] UI_SkillTreeSlot multipleMirageUnlockButton;
    public bool canDuplicateClone { get; private set; }
    private int cloneDuplicateFacingDir = -1;
    [SerializeField][Range(0, 100)] private int percentToDuplicateClone;
    [SerializeField] private float offSetCloneCounterAttack = 1.5f;
    [SerializeField] private float timeDelayCreateCloneCounterAttack = 0.5f;

    [Header("Create crystal instead of clone")]
    [SerializeField] UI_SkillTreeSlot crystalMirageUnlockButton;
    private bool crystalInsteadOfClone;
    public bool CrystalInsteadOfClone { get { return crystalInsteadOfClone; } }
    public void CreateClone(Transform _newTransform, Vector3 _offset, Transform _targetToFacing = null, bool _canDuplicateClone = false, int _percentToDuplicateClone = 0)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.Instance.crystal.CreateCrystal();
            return;
        }
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(_newTransform, cloneDuration, canAtack, _offset, _targetToFacing, _canDuplicateClone, _percentToDuplicateClone);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        //if (canCreateCloneOnCounterAttack)
        //{
        StartCoroutine(CreateCloneWithDelay(0f, _enemyTransform));//create immediately
        //CreateCloneCanDuplicate(_enemyTransform);
        //}
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
