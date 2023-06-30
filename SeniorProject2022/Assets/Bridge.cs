using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {
    private ItemCamera _itemCamera;
    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        _itemCamera = FindObjectOfType<ItemCamera>();
    }
    public void PlayerToItemCam()
    {
        _itemCamera = FindObjectOfType<ItemCamera>();
        _itemCamera.Vcam.m_Lens.FieldOfView = 40;
        _itemCamera.PointCamAtItem(transform);
        
    }

    public void ItemToPlayerCam()
    {
        _itemCamera = FindObjectOfType<ItemCamera>();
        _itemCamera.PointCamBackAtPlayer();
    }
}
