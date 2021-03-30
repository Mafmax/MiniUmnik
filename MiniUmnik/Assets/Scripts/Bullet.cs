using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public Vector3 to = Vector3.zero;
    public Vector3 from=Vector3.zero;
    private void OnDrawGizmos()
    {
       // Gizmos.DrawLine(from, to);
    }
    public void Shoot(Vector3 forward, float force = 12.0f)
    {
        from = transform.position;
        to = transform.position + forward.normalized * 100;
        rb.AddForce(forward.normalized * force);
    }
}
