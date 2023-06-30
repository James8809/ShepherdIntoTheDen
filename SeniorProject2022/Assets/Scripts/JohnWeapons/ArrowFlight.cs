using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFlight : MonoBehaviour
{
    private Rigidbody _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = Vector3.zero;
    }

    // TODO: add charge amount to this from playercontroller class
    public void SetupArrow(PlayerController player, float shootForce)
    {
        _rb.AddRelativeForce(Vector3.forward * shootForce);
    }

    public void Update()
    {
        SpinArrow();
    }

    private void SpinArrow()
    {
        Vector3 vel = _rb.velocity;
        float horizVelocity = Mathf.Sqrt(vel.x * vel.x + vel.z * vel.z);
        float _fallAngle = Mathf.Rad2Deg * -Mathf.Atan2(vel.y, horizVelocity);
        transform.eulerAngles = new Vector3(_fallAngle, transform.eulerAngles.y, transform.eulerAngles.x);
    }
}
