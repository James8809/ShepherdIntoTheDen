using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class ProjectileBehavior : MonoBehaviour
{
    protected Rigidbody _rb;
    [SerializeField] private float lifetime = 5.0f;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // setup rigidbody so I don't have to in editor
        _rb.isKinematic = true;
        _rb.useGravity = false;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
