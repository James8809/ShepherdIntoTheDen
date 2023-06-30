using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    string objName;


    void Awake()
    {
        objName = gameObject.name.Split('(')[0];
        
        DontDestroyOnLoad(this.gameObject);
        PlayerPrefs.SetInt(objName, 1);
        /*
        if (PlayerPrefs.GetInt(name, 0) == 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            PlayerPrefs.SetInt(name, 1);
        }
        */
        //GameObject[] objs = GameObject.FindGameObjectsWithTag("InventoryUI");
        /*
        foreach (GameObject obj in objs)
        {
            if (obj != gameObject)
            {
                Destroy(obj);
            }
        }*/
        /*
        if (objs.Length > 1)
            Destroy(this.gameObject);
        */
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(objName, 0);
    }
}
