using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSound : MonoBehaviour
{
    public void PlayBiteSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Wolf/WolfBite", this.transform.gameObject);
    }

    public void PlayLungeSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Wolf/WolfLunge", this.transform.gameObject);
    }

    public void PlayStalkSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Wolf/WolfStalk", this.transform.gameObject);
    }
}
