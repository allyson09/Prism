using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metal : MonoBehaviour
{
    // Look into making IMagnetic class to inherit from
    private Rigidbody2D rb;
    public float forceMagnitude;

    private bool hasMagnetTarget;
    private Vector3 magnetTargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hasMagnetTarget)
        {
            Attract();
        }
    }

    public void SetMagnetTarget(Vector3 position)
    {
        magnetTargetPosition = position;
        hasMagnetTarget = true;
    }

    private void Attract()
    {

        // direction and distance between the two objects
        Vector2 targetDirection = (magnetTargetPosition - transform.position).normalized;
        rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * forceMagnitude;
        // float distanceToTarget = targetDirection.magnitude;

        // *** Newton's equation for gravity
        // force = the mass of 1 object times the mass of the other object, divided by the distance between them squared. Take that and multiply it by
        // the gravitational constant.
        // float forceMagnitude = (rb.mass * rbToAttract.mass) / distanceToTarget * intensity;
        Vector2 force = targetDirection * forceMagnitude;

        rb.AddForce(force, ForceMode2D.Force);
    }
}
