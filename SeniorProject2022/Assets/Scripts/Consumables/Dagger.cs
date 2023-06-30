using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dagger : MonoBehaviour
{
    private Rigidbody rigidbody;
    public float lifetime = 5;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        DOVirtual.DelayedCall(lifetime, () => Destroy(gameObject));
        DOVirtual.DelayedCall(lifetime - 0.5f, () => transform.DOScale(Vector3.zero, 0.5f)
            .SetEase(Ease.OutBounce));
        rigidbody.AddForce(transform.forward * 1000);
    }
}
