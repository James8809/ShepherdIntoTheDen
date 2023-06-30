using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPositionOnLoad : MonoBehaviour
{
    public Transform spawnPoint;
    Transform actualTransform;
    CharacterController controller;
    
    // Start is called before the first frame update
    void Start()
    {
        actualTransform = GetComponentInChildren<PlayerHealthSystem>().transform;
        controller = GetComponent<CharacterController>();
        SetPosition(spawnPoint.position);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPosition(Vector3 destination)
    {
        controller.transform.position = destination;
    }
}
