using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationController : MonoBehaviour
{
    private GravityObject[] gravityObjects;
    private bool running = true;
    public float timeScale = 1.0f;

    void Awake()
    {
        gravityObjects = FindObjectsOfType<GravityObject>();
    }

    void FixedUpdate()
    {
        if (!running)
            return;

        foreach (GravityObject obj in gravityObjects)
        {
            obj.UpdateVelocity(gravityObjects, Time.fixedDeltaTime * timeScale);
        }

        foreach (GravityObject obj in gravityObjects)
        {
            obj.UpdatePosition(Time.fixedDeltaTime * timeScale);
        }
    }

    public void SetIsRealScale(bool isRealScale)
    {
        foreach (GravityObject obj in gravityObjects)
        {
            obj.isInRealScale = isRealScale;
        }
    }
}
