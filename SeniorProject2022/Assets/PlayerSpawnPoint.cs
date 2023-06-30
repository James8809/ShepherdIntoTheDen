using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityPlayerPrefs;
using DG.Tweening;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject inventoryPrefab;
    public GameObject eventManagerPrefab;
    public bool playWakeUp = true;
    private GameObject playerInstance;
    private GameObject inventoryInstance;
    private GameObject eventManagerInstance;

    PlayerController _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = PlayerController.Instance;
        _player.transform.position = transform.position;
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt(playerPrefab.name, 0) == 0)
        {   
            Debug.Log("cloning playerInst");
            playerInstance = Instantiate<GameObject>(playerPrefab);
            if (playWakeUp)
            {
                playerInstance.GetComponentInChildren<PlayerController>().PlayWakeUpAnimation();
            }
        }
        if (PlayerPrefs.GetInt(inventoryPrefab.name, 0) == 0)
        {
            Debug.Log("cloning inventoryUI Inst");
            inventoryInstance = Instantiate<GameObject>(inventoryPrefab);
        }
        if (PlayerPrefs.GetInt(eventManagerPrefab.name, 0) == 0)
        {
            Debug.Log("cloning eventSystem Inst");
            eventManagerInstance = Instantiate<GameObject>(eventManagerPrefab);
        }
    }

    public void DestroyPermanantObjects()
    {
        Debug.Log("destrouing permanant instances");
        Destroy(playerInstance);
        Destroy(inventoryInstance);
        Destroy(eventManagerInstance);
    }
}
