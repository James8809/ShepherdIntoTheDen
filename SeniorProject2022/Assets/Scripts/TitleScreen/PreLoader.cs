/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLoader : MonoBehaviour
{
    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minimumLogoTime = 3.0f; //minimum time of that scene

    private void Start()
    {
        // Grab the only CanvasGroup in the scene
        fadeGroup = FindObjectsOfType<CanvasGroup>();

        //Start with a white screen;
        fadeGroup.alpha = 1;

        //Pre load the game
        // $$

        //Get a timestamp of the completion time
        //If loadtime is super, give it a small buffer time
        if (Time.time < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
        {
            loadTime = Time.time;
        }

        private void Update()
        //fade in 
        if (Time.time < minimumLogoTime)
        {
            fadeGroup.alpha = 1 - Time.time;
        }

        //fade out
        if (Time.time > minimumLogoTime && loadTime != 0)
        {
            fadeGroup.alpha = Time.time - minimumLogoTime;
            if (fadeGroup.alpha >= 1)
            {
                Debug.Log("Change the scene");
            }
        }
    }
    */