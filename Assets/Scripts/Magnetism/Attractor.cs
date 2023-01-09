using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<Metal>(out Metal metal))
        {
            metal.SetMagnetTarget(transform.parent.position);
        }
    }
}
