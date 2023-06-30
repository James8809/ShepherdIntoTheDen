using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

public class DestroyPlayer : MonoBehaviour
{
    private void Awake()
    {
        var player = FindObjectOfType<PlayerController>();
        var inventoryUI = FindObjectOfType<InventoryManager>();
        
        if (player) Destroy(player.transform.parent.gameObject);
        if (inventoryUI) Destroy(inventoryUI.transform.parent.gameObject);
    }
}
