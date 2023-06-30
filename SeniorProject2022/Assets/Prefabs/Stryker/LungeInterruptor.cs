using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungeInterruptor : MonoBehaviour
{
    public WolfBehaviorTreeManager manager;     // assign in inspector

    private void OnTriggerEnter(Collider other)
    {
        // only weapons that can collide with this system should be registered
        if (manager.lunging)
        {
            manager.InterruptLunge();
        }
    }
}
