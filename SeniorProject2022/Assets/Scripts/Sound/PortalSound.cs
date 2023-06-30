using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSound : MonoBehaviour
{
    void PlayPortalSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Door/Portal/PortalDoorOpen", this.transform.gameObject);
    }
}
