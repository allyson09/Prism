using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticAttraction : MonoBehaviour
{
    // Movement
    public Transform target;
    public float speed;

    // Reference
    // public int magnitudeType;
    // public string objectName;
    // public bool magnetic;
    // public static int Count;
    public int metalObjectId;
    public string metalObjectType;
    public bool isAttractable;
    public string requiredObjectTypeForAttraction;
    public int requiredObjectsForAttraction;
    // variable for color needed
    // variable for number of color needed
    private float distance;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttractable)
        {
            ApplyMagneticForce();
        }
    }

    void ApplyMagneticForce()
    {
        distance = Vector2.Distance(transform.position, target.position);
        Vector2 direction = (target.position - transform.position).normalized;
        // Vector2 direction = (target.position - transform.position).normalized;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg();

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
