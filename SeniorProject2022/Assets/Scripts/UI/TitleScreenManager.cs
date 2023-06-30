using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    public GameObject controlsPrompt;
    public GameObject creditPrompt;
    public Animator transition;
    public float transitionTime;
    public string levelName;

    public void StartGame()
    {
        StartCoroutine(LoadLevel(levelName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClosePrompt()
    {
        controlsPrompt.SetActive(false);
    }

    public void ControlsPromptOpen()
    {
        controlsPrompt.SetActive(true);
    }

    public void CloseCreditPrompt()
    {
        creditPrompt.SetActive(false);
    }

    public void CreditPromptOpen()
    {
        creditPrompt.SetActive(true);
    }

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
