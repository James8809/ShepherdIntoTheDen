using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WolfSceneManager : MonoBehaviour
{
    // Start is called before the first frame update

    private float timer = 0f;
    private float timeToChangeCamera = 5f;
    [SerializeField] private CinemachineVirtualCamera cinemachine;
    private bool canCameraChange = true;
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(timer < timeToChangeCamera){
            timer+= Time.deltaTime;
        } else if(timer > timeToChangeCamera && canCameraChange){
            Debug.Log("change camera ");
            canCameraChange = false;
            cinemachine.m_Priority = -10;
        }
    }
}
