using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public FMODUnity.EventReference swingName;
    public FMODUnity.EventReference stepName;
    public FMODUnity.EventReference rollName;
    public FMODUnity.EventReference deathName;
    public FMODUnity.EventReference stompName;

    public void SwingSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(swingName);
    }

    public void StepSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stepName);
    }

    public void RollSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(rollName);
    }

    public void DeathNoise()
    {
        FMODUnity.RuntimeManager.PlayOneShot(deathName);
    }

    public void StompSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot(stompName);
    }
}
