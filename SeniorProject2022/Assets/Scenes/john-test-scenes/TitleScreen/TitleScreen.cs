using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private float sceneTransitionTime = 5.0f;
    public GameObject creditsCanvases;
    public GameObject controlsCanvases;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }
    
    IEnumerator LoadLevel(string levelName)
    {
        yield return new WaitForSeconds(sceneTransitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void ToggleCredits()
    {
        creditsCanvases.SetActive(!creditsCanvases.activeInHierarchy);
        controlsCanvases.SetActive(false);
    }
    public void ToggleControls()
    {
        controlsCanvases.SetActive(!controlsCanvases.activeInHierarchy);
        creditsCanvases.SetActive(false);
    }
}
