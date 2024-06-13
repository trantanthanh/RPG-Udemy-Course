using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    #region Skills
    public DashSkill dash {  get; private set; }
    public CloneSkill clone { get; private set; }
    public SwordSkill swordThrow { get; private set; }
    public BlackHoleSkill blackHole { get; private set; }
    public CrystalSkill crystal {  get; private set; }
    public ParrySkill parry { get; private set; }
    public DodgeSkill dodge { get; private set; }
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        clone = GetComponent<CloneSkill>();
        swordThrow = GetComponent<SwordSkill>();
        blackHole = GetComponent<BlackHoleSkill>();
        crystal = GetComponent<CrystalSkill>();
        parry = GetComponent<ParrySkill>();
        dodge = GetComponent<DodgeSkill>();
    }
}
