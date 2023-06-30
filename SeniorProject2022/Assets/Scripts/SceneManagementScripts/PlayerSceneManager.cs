using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSceneManager : MonoBehaviour
{
    public GameObject screenUI; 


    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Room1")
        {
            Debug.Log("Room1");
            screenUI.SetActive(false);
            SceneManager.LoadScene("level-1");
            Debug.Log("Scene??");
        }

        if(other.gameObject.tag == "Room2")
        {
            Debug.Log("Room2");
            screenUI.SetActive(false);
            SceneManager.LoadScene("Assets/Scenes/rohan-prototype-scene/LevelPrototypes/level-2.unity");
            Debug.Log("Scene??");
        }

        if(other.gameObject.tag == "Room3")
        {
            Debug.Log("Room3");
            screenUI.SetActive(false);
            SceneManager.LoadScene("Assets/Scenes/rohan-prototype-scene/LevelPrototypes/level-3.unity");
            Debug.Log("Scene??");
        }

        if(other.gameObject.tag == "Room4")
        {
            Debug.Log("Room4");
            screenUI.SetActive(false);
            SceneManager.LoadScene("Assets/Scenes/rohan-prototype-scene/LevelPrototypes/level-4.unity");
            Debug.Log("Scene??");
        }

        if(other.gameObject.tag == "ReturnDoor")
        {
            Debug.Log("Hub");
            screenUI.SetActive(false);
            SceneManager.LoadScene("Assets/Scenes/rohan-prototype-scene/LevelPrototypes/level-hub.unity");
            Debug.Log("Scene??");
        }

        if(other.gameObject.tag == "BossRoom")
        {
            Debug.Log("Boss");
            screenUI.SetActive(false);
            SceneManager.LoadScene("Assets/Scenes/rohan-prototype-scene/LevelPrototypes/level-boss.unity");
            Debug.Log("Scene??");
        }
    }
}
