using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ScareSheep();
    public static event ScareSheep onScared;
    

    public void AttackOccured(Transform loc)
    {
        if (onScared != null)
        {
            onScared();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
