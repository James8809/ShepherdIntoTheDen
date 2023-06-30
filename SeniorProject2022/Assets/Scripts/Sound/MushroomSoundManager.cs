using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSoundManager : MonoBehaviour
{
    public FMODUnity.EventReference attackName;
    // public FMODUnity.EventReference chargeName;
    public FMODUnity.EventReference alertName;
    public FMODUnity.EventReference explosionName;
    //private FMOD.Studio.EventInstance attackEvent;
    //private FMOD.Studio.EventInstance chargeEvent;
    //private FMOD.Studio.EventInstance alertEvent;
    private FMOD.Studio.EventInstance explosionEvent;
    // public FMODUnity.EventReference deathName;

    private void Start() {
        explosionEvent = FMODUnity.RuntimeManager.CreateInstance(explosionName);
        //attackEvent = FMODUnity.RuntimeManager.CreateInstance(attackName);
        //chargeEvent = FMODUnity.RuntimeManager.CreateInstance(chargeName);
        //alertEvent = FMODUnity.RuntimeManager.CreateInstance(alertName);
    }

    private void Update() {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(explosionEvent, GetComponent<Transform>(),GetComponent<Rigidbody>());
    }

    public void AttackSound()
    {
        // FMODUnity.RuntimeManager.PlayOneShot(attackName);
    }

    public void ChargeSound()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Enemy/Mushroom/Charge/MushroomCharge1", transform.parent.gameObject);
    }

    public void AlertSound()
    {
        // FMODUnity.RuntimeManager.PlayOneShot(alertName, GetComponent<GameObject>());
    }

    public void ExplosionNoise()
    {
        explosionEvent.start();
    }

    // public void DeathNoise()
    // {
    //     FMODUnity.RuntimeManager.PlayOneShot(deathName);
    // }
}
