using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private GravityObject[] gravityObjects;

    void Awake()
    {
        gravityObjects = FindObjectsOfType<GravityObject>();
    }

    void FixedUpdate()
    {
        foreach (GravityObject obj in gravityObjects)
        {
            obj.UpdateVelocity(gravityObjects, Time.fixedDeltaTime);
        }

        foreach (GravityObject obj in gravityObjects)
        {
            obj.UpdatePosition(Time.fixedDeltaTime);
        }
    }
}
