using UnityEngine;

public class LockedCameraController : MonoBehaviour
{
    public Transform target; // the planet game object that the camera will follow
    public float distance = 10f; // the distance between the camera and the planet
    public float height = 5f; // the height of the camera above the planet
    public float rotateSpeed = 1f; // the speed of camera rotation
    public float zoomSpeed = 20f;
    public float fastZoomSpeed = 200f;

    private Vector3 offset; // the offset of the camera from the planet
    private float currentDistance; // the current distance between the camera and the planet
    private Quaternion currentRotation; // the current rotation of the camera

    private void Start()
    {
        offset = transform.position - target.position;
        currentDistance = distance;
        currentRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // handle mouse input for camera rotation
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float rotateHorizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            float rotateVertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            currentRotation *= Quaternion.Euler(-rotateVertical, rotateHorizontal, 0f);
        }

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance = Mathf.Clamp(currentDistance - mouseScroll * (Input.GetKey(KeyCode.LeftControl) ? fastZoomSpeed : zoomSpeed), 20f, 3000f);

        // calculate the desired position of the camera
        Vector3 desiredPosition = target.position + currentRotation * (offset.normalized * currentDistance + Vector3.up * height);

        transform.position = desiredPosition;

        // make the camera look at the planet
        transform.LookAt(target);
    }
}
