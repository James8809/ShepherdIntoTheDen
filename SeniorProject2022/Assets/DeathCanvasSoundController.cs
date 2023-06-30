using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCanvasSoundController : MonoBehaviour
{
    private void OnEnable() {
        FMODUnity.RuntimeManager.MuteAllEvents(true);
    }

    private void OnDisable() {
        FMODUnity.RuntimeManager.MuteAllEvents(false);
    }
}
