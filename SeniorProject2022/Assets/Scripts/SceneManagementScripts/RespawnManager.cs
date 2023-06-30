using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    PlayerController controller;
    public string TitleScreenName = "TitleScreen";
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        controller.Respawn();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private IEnumerator WaitToGoToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        // destroy the player prefab
        FindObjectOfType<PlayerSpawnPoint>().DestroyPermanantObjects();
        SceneManager.LoadScene(TitleScreenName);
    }

    public void GoToMainMenu(float delay)
    {
        StartCoroutine(WaitToGoToMainMenu(delay));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
