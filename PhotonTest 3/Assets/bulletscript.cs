using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletscript : MonoBehaviour
{
    public bool UseVelocityDmg;
    public float BaseDmg;
    public float VelocityDmgMultiplier;
    public float Damage;

    void Start()
    {
        Damage = BaseDmg;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (UseVelocityDmg)
        {
            Damage = BaseDmg + (collision.relativeVelocity.magnitude * VelocityDmgMultiplier);
        }
    }

}
