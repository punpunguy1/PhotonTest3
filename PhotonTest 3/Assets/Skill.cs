using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Skill", menuName = "New Skill")]
public class Skill : ScriptableObject
{

    #region Variables
    [Header("Stats")]
    public float Cooldowntime;
    public float Damage;
    public float skillforce;
    public float spread;
    public float cost;
    public float freezetime;
    public bool freezerot;

    public bool UseTrailingFreeze;
    public bool FireMultiple;
    public int FireCount;
    public float TimeInBetween;
    public float FreezeTrail;
    #endregion
    
    #region Objects
    [Header("Objects")]
    public bool IsProjectile;
    public GameObject skillpf;
    public AudioClip skillsound;
    #endregion
}
