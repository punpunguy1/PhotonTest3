using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Item item;
    [Header("Powerups")]

    //0- cumregen, 1-addsperm, 2-addhealth, 3-addhealthregen
    public float[] buffs;
    public float addCumRegen;
    public float addSpermCount;
    public float addHealth;
    public float addHealthRegen;

    public string PowerUpName;
    //Sprites and UI
    public Sprite powerupicon;
    private void Awake()
    {
        buffs = new float[item.buffs.Length];
        buffs = item.buffs;
        addCumRegen = item.addCumRegen;
        addSpermCount = item.addSpermCount;
        addHealth = item.addHealth;
        addHealthRegen = item.addHealthRegen;
    }
}
