using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WolfPackManager : MonoBehaviour
{
    public WolfBehaviorTreeManager[] wolves;
    public float detectionRadius;
    int numWolvesAlive;
    Collider sensor;
    public Animator portalAnimator;

    private void Start()
    {
        sensor = GetComponent<Collider>();
        sensor.enabled = false;
        foreach (WolfBehaviorTreeManager wolf in wolves)
        {
            wolf.packManager = this;
        }
        numWolvesAlive = wolves.Length;
        StartCoroutine(EnableSensorAfterSeconds(0.5f));
    }

    public IEnumerator EnableSensorAfterSeconds(float sec)
    {
        yield return new WaitForSeconds(sec);
        sensor.enabled = true;
    }

    public void WolfDown()
    {
        numWolvesAlive--;
        if (numWolvesAlive == 0)
        {
            //SceneManager.LoadScene("WinScene");
            Debug.Log("victory!");
            portalAnimator.SetTrigger("OpenDoor");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Transform>().CompareTag("Player"))
        {
            Debug.Log("player entered");
            EnterCombat();
        }
    }

    public void EnterCombat()
    {
        foreach (WolfBehaviorTreeManager wolf in wolves)
        {
            wolf.inCombat = true;
        }
    }
}
