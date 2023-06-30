using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepTrap : MonoBehaviour
{
    private Animator _anim;
    public FMODUnity.EventReference rewardSound;

    public SheepController _Sheep;  // enabled after door is opened.
    [SerializeField] private EnemyCampTracker _enemyCampTracker;    // has to be set in inspector
    private void Awake()
    {
        // subscribe to all dead event.
        if (_enemyCampTracker != null)
        {
            _enemyCampTracker.OnAllEnemiesDead += OnCampDeafeated;
        }

        // yet another singleton thing :/
        _anim = GetComponent<Animator>();
    }

    public void OnCampDeafeated()
    {
        _anim.SetTrigger("OpenDoor");
        if (_Sheep != null)
        {
            _Sheep.EnablePickup();
        }
    }
    

    public void PlayerToItemCam()
    {
        ItemCamera _itemCamera = FindObjectOfType<ItemCamera>();
        _itemCamera.Vcam.m_Lens.FieldOfView = 16;
        _itemCamera.PointCamAtItem(transform);
    }

    public void ItemToPlayerCam()
    {
        FindObjectOfType<ItemCamera>().PointCamBackAtPlayer();
    }

    public void PlaySound(){
        FMODUnity.RuntimeManager.PlayOneShot(rewardSound);
    }
}