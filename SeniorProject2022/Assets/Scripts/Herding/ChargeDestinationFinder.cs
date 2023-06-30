using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeDestinationFinder : MonoBehaviour
{
    public bool hasHitEnvironment;
    public bool hasHitEnemy;
    public Transform target;
    public Vector3 direction;
    public Vector3 origin;

    float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        hasHitEnvironment = false;
        hasHitEnemy = false;
        origin = transform.position;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        if (direction != null && !hasHitEnvironment)
        {
            transform.position += direction * speed; ;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (!hasHitEnemy && !hasHitEnvironment)
            {
                target = other.transform;
                transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                Vector3 newDir = transform.position - origin;
                newDir.Normalize();
                SetDirection(newDir);
                
            }
        }
        else if (other.gameObject.isStatic)
        {
            hasHitEnvironment = true;
        }
    }
}
