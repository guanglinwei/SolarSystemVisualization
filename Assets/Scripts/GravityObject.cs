using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GravityObject : MonoBehaviour
{
    private const double gravConstant = 6.67408e-10;
    private Rigidbody rb;

    [HideInInspector]
    public Vector3 velocity;

    [HideInInspector]
    public float rotationSpeed;

    public Vector3 initialVelocity = Vector3.zero; // For circular orbit, v = sqrt(G*M/r)
    public float mass = 1f;
    public float radius = 1f;

    [HideInInspector]
    public float realScaleRadius = 1f;
    [HideInInspector]
    public bool isInRealScale = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        velocity = initialVelocity;
    }

    private void Update()
    {
        transform.localScale = Vector3.one * (isInRealScale ? realScaleRadius : radius);
    }

    public void UpdateVelocity(GravityObject[] objects, float deltaTime)
    {
        foreach (GravityObject obj in objects)
        {
            if (obj != this)
            {
                Vector3 vec = obj.rb.position - rb.position;
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
        transform.Rotate(0f, rotationSpeed * deltaTime, 0f);
    }
}
