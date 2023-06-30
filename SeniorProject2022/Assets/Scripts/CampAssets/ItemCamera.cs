using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ItemCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    public CinemachineVirtualCamera Vcam
    {
        get => vcam;
        set => vcam = value;
    }

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void PointCamAtItem(Transform item)
    {
        vcam.Follow = item;
        //vcam.LookAt = item;
        vcam.Priority = 100;
    }
    

    public void PointCamBackAtPlayer()
    {
        vcam.Priority = 0;
    }
}