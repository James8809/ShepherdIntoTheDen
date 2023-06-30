using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Crafting;
using Random = System.Random;

public class Collectable : MonoBehaviour
{
    public Item item;
    private InventoryManager inventoryManager;
    private Tween doTween;
    private Tween doTween2;

    void Start()
    {

        inventoryManager = FindObjectOfType<InventoryManager>(true);
        Debug.Log("trying to find inventoryManager");
        Debug.Log(inventoryManager);
        StartCoroutine(WaitAndStartMoveTween());
    }

    // Weapon Collides with object
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            return;
        FMODUnity.RuntimeManager.PlayOneShot("event:/ResourceDrop/Pickup");
        inventoryManager.AddItem(item, 1);
        NotificationManager.Instance.CreateNotification(item);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/ResourceDrop/Pickup", this.transform.gameObject);
        doTween2 = transform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBounce)
            .OnComplete(() => {
                doTween2.SetTarget(null);
                doTween.Kill();
                doTween2.Kill();
                Destroy(this.gameObject);
            });
        //Destroy(this.gameObject);
    }

    // Up and down animation
    private IEnumerator WaitAndStartMoveTween()
    {
        yield return new WaitForSeconds(1f);
        doTween = transform.DOLocalMoveY(transform.position.y + 0.3f, 1, false)
            .SetLoops(-1, LoopType.Yoyo)
            .SetDelay(UnityEngine.Random.Range(0.2f, 0.5f))
            .SetEase(Ease.InOutSine);
    }

}
