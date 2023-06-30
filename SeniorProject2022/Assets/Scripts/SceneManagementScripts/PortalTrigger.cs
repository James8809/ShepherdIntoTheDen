using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalTrigger : MonoBehaviour
{
    public string levelName;
    public float transitionTime;
    public Image fadeToWhitePanel;
    private void OnTriggerEnter(Collider other)
    {
        //StartCoroutine(LoadLevel(levelName));
        // SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        if(other.tag != "Player"){
            return;
        }
        if (fadeToWhitePanel)
        {
            fadeToWhitePanel.DOFade(1, 2.0f).OnComplete(() =>
            {
                SceneManager.LoadScene(levelName, LoadSceneMode.Single);
                PlayerController.Instance.PlayWakeUpAnimation();
            });
            
            return;
        }
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
    
    IEnumerator LoadLevel(string levelName)
    {
        // sorry rohan idk what this is man...
        // transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }
}
