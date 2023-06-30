using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    [HideInInspector]public RectTransform rectTransform;
    [HideInInspector]public Vector3 originalPos;
    [HideInInspector]public CanvasGroup canvasGroup;
    public string abilityName;
    public bool isSlot;
    public Image image;
    public GameObject text = null;
    public ItemSlot itemSlot;
    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        originalPos = image.rectTransform.anchoredPosition;
        canvasGroup = this.GetComponent<CanvasGroup>();

    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        // if(checkCanMove())
        // {
        //     canvasGroup.blocksRaycasts = false;
        //     canvasGroup.alpha = 0.5f;
        // }
        if (checkCanDrag())
        {
            FindObjectOfType<InventoryInputManager>(true).DisableUIInput();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0.5f;
            if(text != null)
            {
                text.SetActive(false);
            }
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (checkCanDrag())
        {
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position
                , eventData.pressEventCamera, out var globalMousePosition)) {
                    image.rectTransform.position = globalMousePosition;
                }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnMissed();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // need this interface to work even no code
    }


    public void OnMissed()
    {
        if(checkCanDrag())
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            if(text != null)
            {
                text.SetActive(true);
            }
        
            image.rectTransform.anchoredPosition = originalPos;
            if(isSlot){
                var slot = this.transform.parent.gameObject.GetComponent<ItemSlot>();//
                slot.OnDropOut();
            }
            FindObjectOfType<AbilitiesUI>().RefreshAbilitiesTab();
        }
        // canvasGroup.alpha = 1f;
        // canvasGroup.blocksRaycasts = true;

    
        // rectTransform.anchoredPosition = originalPos;
        // if(isSlot){
        //     var slot = this.transform.parent.gameObject.GetComponent<ItemSlot>();//
        //     slot.OnDropOut();
        // }
        // FindObjectOfType<AbilitiesUI>().RefreshAbilitiesTab();
    }

    public bool checkCanDrag()
    {
        return FindObjectOfType<InventoryUI>(true).gameObject.activeSelf;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        FindObjectOfType<InventoryInputManager>(true).EnableUIInput();
    }

    public void OnUnequip()
    {
        if (itemSlot!=null)
        {
            itemSlot.OnDropOut();
        }
        itemSlot = null;
    }
}