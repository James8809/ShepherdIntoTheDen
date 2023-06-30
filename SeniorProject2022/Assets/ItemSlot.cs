using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private PlayerController player;
    private bool hasImage;
    private GameObject prevImage;
    [SerializeField]private Image image;
    [SerializeField] private Image imageObj;
    public string keyName;
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if(eventData.pointerDrag.GetComponentInChildren<DragDrop>().isSlot) {
                var dragDrop = prevImage.GetComponentInChildren<DragDrop>();
                dragDrop.transform.gameObject.SetActive(true);
                player.abilityState.keyMapDisable(keyName, prevImage.GetComponentInChildren<DragDrop>().abilityName);
                prevImage = eventData.pointerPress;
                prevImage.GetComponentInChildren<DragDrop>().OnMissed();
                image.enabled = false;
                hasImage = false;
            }
            else
            {
                if(hasImage){
                    prevImage.SetActive(true);
                    player.abilityState.keyMapDisable(keyName, prevImage.GetComponentInChildren<DragDrop>().abilityName);
                }
                prevImage = eventData.pointerPress;
                var dragDrop = prevImage.GetComponentInChildren<DragDrop>();
                dragDrop.OnMissed();
                player.abilityState.keyMapEnable(keyName, prevImage.GetComponentInChildren<DragDrop>().abilityName);
                image.enabled = true;
                //image.sprite = prevImage.GetComponent<Image>().sprite;
                image.sprite = dragDrop.image.sprite;
                //dragDrop.transform.parent.gameObject.SetActive(false);
                dragDrop.itemSlot = this;
                dragDrop.transform.gameObject.SetActive(false);
                hasImage = true;
            }
        }
    }

    public void OnDropOut()
    {
        
        prevImage.SetActive(true);
        player.abilityState.keyMapDisable(keyName, prevImage.GetComponentInChildren<DragDrop>().abilityName);
        //prevImage.GetComponentInChildren<DragDrop>().OnMissed();
        image.enabled = false;
        hasImage = false;
    }
}
