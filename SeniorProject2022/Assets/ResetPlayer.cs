using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour
{
    GameObject player;
    GameObject UI;
    
    // Start is called before the first frame update
    void Start()
    {
        UI = GameObject.FindObjectOfType<InventoryManager>().transform.root.gameObject;
        player = GameObject.FindObjectOfType<PlayerController>().transform.root.gameObject;

        Debug.Log(UI);
        Debug.Log(player);

        Destroy(player);
        Destroy(UI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
