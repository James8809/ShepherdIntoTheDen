using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting;
using DG.Tweening;

public class Scroll : MonoBehaviour
{
    public CraftableItem item;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(true);
        transform.DOLocalMoveY(transform.position.y + 0.03f, 1, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(UnityEngine.Random.Range(0.2f, 0.5f))
            .SetEase(Ease.InOutSine);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/UI_Open");
        // Add craftable
        inventoryManager.AddRecipeToCraftableItems(item);
        // notification
        transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBounce)
            .OnComplete(() => Destroy(this.gameObject));
        NotificationManager.Instance.CreatePopupNotification(item);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/ResourceDrop/Pickup", this.transform.gameObject);
        GetComponentInChildren<SphereCollider>().enabled = false;
    }
}
