using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item")]

public class Item : ScriptableObject
{
    #region Powerups
    [Header("Powerups")]
    public float[] buffs;
    public float addCumRegen;
    public float addSpermCount;
    public float addHealth;
    public float addHealthRegen;

    #endregion
}
