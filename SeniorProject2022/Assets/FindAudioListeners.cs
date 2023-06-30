using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindAudioListeners : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioListener[] listeners = GameObject.FindObjectsOfType<AudioListener>();
        foreach(AudioListener list in listeners)
        {
            Debug.Log(list.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
