using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyconidSound : MonoBehaviour
{
    public void StompSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Giant Mushroom/Explosion/GiantMushroomFall1", transform.parent.gameObject);
    }

    public void ChargeSound(){
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Giant Mushroom/Charge/GiantMushroomCharge1", transform.parent.gameObject);
    }
}
