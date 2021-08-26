using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour
{
    public bool specialattacking;

    public void SpecialAttack()
    {
        specialattacking = true;

    }
    public void ExitSpecial()
    {
        specialattacking = false;
    }


    
}
