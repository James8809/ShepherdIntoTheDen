using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crafting;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NotificationManager : MonoBehaviour
{
    [Header("Drag GameObjects")]
    public GameObject NotificationContainer;
    public GameObject PopupContainer;
    
    [Header("Prefabs")]
    public GameObject NotificationPanel;

    [Header("Variables")] 
    public float notificationTime;

    private Dictionary<Item, GameObject> currentNotifications = new Dictionary<Item, GameObject>();
    private Dictionary<Item, Coroutine> currentCoroutines = new Dictionary<Item, Coroutine>();
    private InventoryInputManager inventoryInputManager;
    private ManagerUI managerUI;
    private PlayerInput playerInput;

    
    // Dummy singleton (o)-(o)
    public static NotificationManager Instance { get; private set; }
    private void Awake() 
    { 
        playerInput = new PlayerInput();

        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    private void Start()
    {
        inventoryInputManager = FindObjectOfType<InventoryInputManager>();
        inventoryInputManager.closeTab += ClosePopupTab;
        managerUI = FindObjectOfType<ManagerUI>();
    }

    public void CreateNotification(Item item)
    {
        // New notification
        if (!currentNotifications.ContainsKey(item))
        {
            var notification = Instantiate(NotificationPanel, NotificationContainer.transform);
            notification.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
            notification.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;
            notification.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text = "x1";
            
            currentNotifications[item] = notification;
            currentCoroutines[item] = StartCoroutine(RemoveNotification(item));
            return;
        }
        
        // Notification already exists
        var notif = currentNotifications[item];
        var text = notif.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text;
        var newNum = int.Parse(text.Remove(0, 1)) + 1;
        notif.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>().text = "x" + newNum;
        StopCoroutine(currentCoroutines[item]);
        currentCoroutines[item] = StartCoroutine(RemoveNotification(item));
    }

    private IEnumerator RemoveNotification(Item item)
    {
        yield return new WaitForSeconds(notificationTime);
        currentNotifications[item].SetActive(false);
        Destroy(currentNotifications[item].gameObject);
        currentNotifications.Remove(item);
    }

    public void CreatePopupNotification(CraftableItem item)
    {
        PopupContainer.GetComponent<CanvasGroup>().alpha = 0;
        PopupContainer.GetComponent<CanvasGroup>().DOFade(1, 2).SetUpdate(true);
        var popupPanel = PopupContainer.transform.GetChild(0);
        var image = popupPanel.transform.Find("Image").GetComponent<Image>();
        var itemName = popupPanel.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemDescription = popupPanel.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();

        image.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.description;

        Time.timeScale = 0;
        PopupContainer.SetActive(true);
        managerUI.DisablePauseInput();
        inventoryInputManager.DisableUIInput();
        EnableNotificationUIInput();
    }

    public void ClosePopupTab()
    {
        managerUI.EnablePauseInput();
        Time.timeScale = 1;
        PopupContainer.GetComponent<CanvasGroup>().DOFade(0, 0.2f).OnComplete(()=> PopupContainer.SetActive(false));
        inventoryInputManager.EnableUIInput();
        DisableNotificationUIInput();
    }

    private void ClosePopupTabCallback(InputAction.CallbackContext context)
    {
        ClosePopupTab();
    }

    public void EnableNotificationUIInput()
    {
        playerInput.UIControls.Enable();
        playerInput.UIControls.CloseNotification.started += ClosePopupTabCallback;
    }

    public void DisableNotificationUIInput()
    {
        playerInput.UIControls.Disable();
        playerInput.UIControls.CloseNotification.started -= ClosePopupTabCallback;
    }
}
