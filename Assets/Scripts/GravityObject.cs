using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    private const double gravConstant = .5;//6.67408e-10;
    private Rigidbody rb;

    [HideInInspector]
    public Vector3 velocity;

    public Vector3 initialVelocity = Vector3.zero; // For circular orbit, v = sqrt(G*M/r)
    public float mass = 1;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = initialVelocity;
    }

    public void UpdateVelocity(GravityObject[] objects, float deltaTime)
    {
        foreach (GravityObject obj in objects)
        {
            if (obj != this)
            {
                Vector3 vec = obj.rb.position - this.rb.position;
                float sqrDist = vec.sqrMagnitude;
                Vector3 dir = vec.normalized;

                velocity += deltaTime * dir * (float)gravConstant * obj.mass / sqrDist;
                // rb.AddForce(dir * (float)gravConstant * obj.rb.mass / sqrDist);
            }
        }
    }

    public void UpdatePosition(float deltaTime)
    {
        rb.MovePosition(rb.position + velocity * deltaTime);
    }
}
