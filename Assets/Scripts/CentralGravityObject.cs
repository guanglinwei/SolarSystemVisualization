using UnityEngine;

public class CentralGravityObject : GravityObject
{
    private OrbitContainerController orbitContainerController;

    public override void Awake()
    {
        base.Awake();
        orbitContainerController = GetComponentInChildren<OrbitContainerController>();
    }

    // The orbital lines must be rotated in the opposite direction so they do not also move
    public override void UpdateRotation(float deltaTime)
    {
        base.UpdateRotation(deltaTime);
        // orbitContainerController.transform.rotation = Quaternion.Inverse(currentRotation);
        orbitContainerController.transform.localRotation = Quaternion.Euler(-1 * currentRotation.eulerAngles);
    }

    public void SetChildOrbitLinesVisible(bool visible)
    {
        orbitContainerController.gameObject.SetActive(visible);
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SetActive(visible);
        //}
    }
}
