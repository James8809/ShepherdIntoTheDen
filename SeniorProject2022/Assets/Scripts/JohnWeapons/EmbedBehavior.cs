using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbedBehavior : MonoBehaviour
{
    private Rigidbody _rb;
    public float TimeToDestroyAfterEmbed = 5.0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Embed();
    }

    void Embed()
    {
        transform.GetComponent<ArrowFlight>().enabled = false;
        _rb.velocity = Vector3.zero;
        _rb.useGravity = false;
        _rb.isKinematic = true;
        Destroy(gameObject, TimeToDestroyAfterEmbed);
    }
}
