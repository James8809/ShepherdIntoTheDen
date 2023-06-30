using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death_Screen : MonoBehaviour
{
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

    IEnumerator LoadLevel(string levelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
