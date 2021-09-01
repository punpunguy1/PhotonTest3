using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

[CreateAssetMenu(fileName = "New Character", menuName = "Player Character")]
public class Player : ScriptableObject
{
    #region Declares

#region Player Stats

    [Header("Player Stats")]
    public float startHealth;
    public float startRegenrate;
    public float MoveSpeed;
    //Combat Stats
    public float startCumMax;
    public float startCumamount;
    public float startCumrate;
    #endregion

#region Skills

    [Header("Skills")]
    public List<Skill> Skills;
    #endregion

#region Animation and Sprites

    [Header("Animation and Sprites")]
    public Sprite idleSprite;
    public Animator animator;
    public float spriteRotation;
    #endregion

    #region Audio

    public AudioClip[] hitsound;
    #endregion


    #endregion



}
