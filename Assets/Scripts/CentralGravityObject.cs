using UnityEngine;

public class CentralGravityObject : GravityObject
{
    public void SetChildOrbitLinesVisible(bool visible)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(visible);
        }
    }
}
