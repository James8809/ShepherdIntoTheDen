using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject controlsPrompt;
    private bool isPaused = false;
    private PlayerInput _playerInput;
    [SerializeField] private PlayerReferenceManager _referenceManager;
    public Camera deathCam;
    public Canvas deathScreen;

    public void EnableDeathUI()
    {
        deathCam.gameObject.SetActive(true);
        deathScreen.gameObject.SetActive(true);
    }

    public void DisableDeathUI()
    {
        deathCam.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _referenceManager.uiManager = this;
    }

    private void OnDisable()
    {
        _referenceManager.uiManager = null;
    }

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        EnablePauseInput();
    }

    void TogglePause(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;
        if (isPaused)
            Pause();
        else
            Resume();
    }

    public void Resume()
    {
        PlayerController.Instance.EnableInput();
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        if (FindObjectOfType<InventoryInputManager>())
            FindObjectOfType<InventoryInputManager>().EnableUIInput();
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void ControlsPrompt()
    {
        controlsPrompt.SetActive(true);
    }
    public void Pause()
    {
        PlayerController.Instance.DisableInput();
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        if (FindObjectOfType<InventoryInputManager>())
            FindObjectOfType<InventoryInputManager>().DisableUIInput();
    }
    public void ClosePrompt()
    {
        controlsPrompt.SetActive(false);
    }

    public void EnablePauseInput()
    {
        _playerInput.CharacterControls.Pause.started += TogglePause;
    }

    public void DisablePauseInput()
    {
        _playerInput.CharacterControls.Pause.started -= TogglePause;
    }

}
