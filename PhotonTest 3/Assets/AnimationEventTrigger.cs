using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    public AudioSource whyhaveu;
    public AudioSource coo;
    public AudioSource ooo;
    public AudioSource oom;
    public bool mute;
    public bool specialattacking;
    public void playwhy()
    {
        if (!whyhaveu.isPlaying)
        {
            whyhaveu.Play();
        }
        
    }
    public void playcoooo()
    {
        if (!mute)
        {
            if (!coo.isPlaying)
            {
                coo.Play();
            }
        }
        

    }
    public void playooo()
    {
        if (!mute)
        {
            if (!ooo.isPlaying)
            {
                ooo.Play();
            }

        }

    }
    public void playoom()
    {
        if (!mute)
        {
            if (!oom.isPlaying)
            {
                oom.Play();
            }
        }
        

    }
    public void SpecialAttack()
    {
        specialattacking = true;

    }
    public void ExitSpecial()
    {
        specialattacking = false;
    }


    
}
