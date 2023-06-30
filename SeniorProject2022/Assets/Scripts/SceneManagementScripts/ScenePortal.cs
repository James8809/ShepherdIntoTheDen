using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScenePortal : MonoBehaviour
{
    private Animator _anim;
    private ItemCamera _itemCamera;

    private void Awake()
    {
        _itemCamera = FindObjectOfType<ItemCamera>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //FindObjectOfType<SheepCollectionUI>().OnAllSheepCollected += PlayDoorAnim;
        var dialogue = FindObjectOfType<DialogueManager>();
        if(dialogue != null)
        {
            dialogue.OnFinishTutorial += PlayDoorAnim;
        }
    }

    private void OnDisable()
    {
        //FindObjectOfType<SheepCollectionUI>().OnAllSheepCollected -= PlayDoorAnim;
    }

    private void PlayDoorAnim()
    {
        if(_itemCamera == null){
            _itemCamera = FindObjectOfType<ItemCamera>();
        }
        _itemCamera.Vcam.m_Lens.FieldOfView = 25;
        _anim.SetTrigger("OpenDoor");
    }

    public void PlayerToItemCam()
    {
        _itemCamera = FindObjectOfType<ItemCamera>();
        _itemCamera.Vcam.m_Lens.FieldOfView = 16;
        _itemCamera.PointCamAtItem(transform);
    }

    public void ItemToPlayerCam()
    {
        _itemCamera.Vcam.m_Lens.FieldOfView = 16;
        _itemCamera.PointCamBackAtPlayer();
    }
}
