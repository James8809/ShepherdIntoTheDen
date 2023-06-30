using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class InventoryInputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerController playerController;
    private InventoryUI inventoryUI;
    private LoadoutUI loadoutUI;
    private ManagerUI managerUI;


    public Action closeTab;

    private bool isInventoryEnabled = false;

    private void Awake()
    {
        playerInput = new PlayerInput();
        managerUI = FindObjectOfType<ManagerUI>();

        Initialize();
        
        if (!inventoryUI)
            Debug.LogError("No InventoryUI Game Object in scene. Please add one.");

        SceneManager.sceneLoaded += InitializeScene;
    }

    private void InitializeScene(Scene scene, LoadSceneMode load)
    {
        // need to find new playerController
        // TODO: delete when player becomes don't destroy on load
        playerController = FindObjectOfType<PlayerController>(true);
    }
    
    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>(true);
        inventoryUI = FindObjectOfType<InventoryUI>(true);
        loadoutUI = FindObjectOfType<LoadoutUI>(true);
        EnableUIInput();
    }
    
    #region UI Input
    
    void OnEnable()
    {
        EnableUIInput();
    }

    void OnDisable()
    {
        DisableUIInput();
    }
    
    // Inventory KeyBinds
    void OnToggleInventory(InputAction.CallbackContext context)
    {
        Debug.Log("tabbingggg");
        ToggleInventoryTab();
    }

    void ToggleInventoryTab()
    {
        isInventoryEnabled = inventoryUI.ToggleInventory();
        if (!playerController)
            playerController = FindObjectOfType<PlayerController>(true);
        if (!managerUI)
            FindObjectOfType<ManagerUI>();
        if (isInventoryEnabled)
        {
            playerController.enabled = false;
            managerUI.DisablePauseInput();
        }
        else
        {
            playerController.enabled = true;
            managerUI.EnablePauseInput();
        }
    }

    // Loadout KeyBinds
    private void UseLoadout0(InputAction.CallbackContext context)
    {
        loadoutUI.UseLoadout(0);
    }
    private void UseLoadout1(InputAction.CallbackContext context)
    {
        loadoutUI.UseLoadout(1);
    }
    private void UseLoadout2(InputAction.CallbackContext context)
    {
        loadoutUI.UseLoadout(2);
    }
    private void UseLoadout3(InputAction.CallbackContext context)
    {
        loadoutUI.UseLoadout(3);
    }

    private void CloseTab(InputAction.CallbackContext context)
    {
        if (isInventoryEnabled)
        {
            ToggleInventoryTab();
        }
        closeTab?.Invoke();
    }
    
    public void EnableUIInput()
    {
        // setup callbacks for UI
        playerInput.UIControls.Enable();
        playerInput.UIControls.ToggleInventory.started += OnToggleInventory;
        playerInput.UIControls.UseLoadout0.started += UseLoadout0;
        playerInput.UIControls.UseLoadout1.started += UseLoadout1;
        playerInput.UIControls.UseLoadout2.started += UseLoadout2;
        playerInput.UIControls.UseLoadout3.started += UseLoadout3;

        playerInput.UIControls.CloseTab.started += CloseTab;
    }

    public void DisableUIInput()
    {
        playerInput.UIControls.ToggleInventory.started -= OnToggleInventory;
        playerInput.UIControls.UseLoadout0.started -= UseLoadout0;
        playerInput.UIControls.UseLoadout1.started -= UseLoadout1;
        playerInput.UIControls.UseLoadout2.started -= UseLoadout2;
        playerInput.UIControls.UseLoadout3.started -= UseLoadout3;
        playerInput.UIControls.CloseTab.started -= CloseTab;
        playerInput.UIControls.Disable();
    }
    #endregion

}    
