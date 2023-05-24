using UnityEngine;

public class LockedCameraController : MonoBehaviour
{
    public Transform target; // the planet game object that the camera will follow
    public float distance; // the distance between the camera and the planet
    public float height; // the height of the camera above the planet
    public float rotateSpeed; // the speed of camera rotation
    public float zoomSpeed;
    public float fastZoomSpeed;

    private Vector3 offset; // the offset of the camera from the planet
    private float currentDistance; // the current distance between the camera and the planet
    private Quaternion currentRotation; // the current rotation of the camera

    private bool doLerp = false;
    private float lerpTime = 1f;
    private float currentLerpTime = 0f;
    private float lerpTimeScale = 1f;
    private Vector3 lastCameraPositionBeforeLerp;
    private Quaternion lastCameraRotationBeforeLerp;

    [HideInInspector]
    public bool isRealPlanetScale = false;

    private void Start()
    {
        offset = transform.position - target.position;
        currentDistance = distance;
        currentRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // calculate the desired position of the camera
        Vector3 desiredPosition = target.position + currentRotation * (offset.normalized * currentDistance + Vector3.up * height);
        
        if (Random.Range(0f, 1f) <= 0.2f)
        {
            //Debug.Log(Vector3.Distance(transform.position, desiredPosition) + "  " + lerpTime);
        }

        if (Vector3.Distance(transform.position, desiredPosition) <= 10f)
        {
            doLerp = false;
        }

        if (doLerp)
        {
            transform.position = Vector3.Lerp(lastCameraPositionBeforeLerp, desiredPosition, currentLerpTime);
            Quaternion desiredRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(lastCameraRotationBeforeLerp, desiredRotation, currentLerpTime);

            lerpTimeScale = Mathf.Max(0.2f, lerpTimeScale - Time.deltaTime * .2f);
            currentLerpTime = Mathf.Min(1f, currentLerpTime + lerpTimeScale * Time.deltaTime / lerpTime);

            
        } else
        {
            // handle mouse input for camera rotation
            if (Input.GetKey(KeyCode.Mouse0))
            {
                float rotateHorizontal = Input.GetAxis("Mouse X") * rotateSpeed;
                float rotateVertical = Input.GetAxis("Mouse Y") * rotateSpeed;
                currentRotation *= Quaternion.Euler(-rotateVertical, rotateHorizontal, 0f);
            }

            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            float currentZoomSpeed = Mathf.Max(isRealPlanetScale ? 0.01f : 1f, Mathf.Sqrt(currentDistance / 160f)) * (Input.GetKey(KeyCode.LeftControl) ? fastZoomSpeed : zoomSpeed);
            currentDistance = Mathf.Clamp(currentDistance - mouseScroll * currentZoomSpeed, 0.1f, 3000f);

            // calculate the desired position of the camera
            desiredPosition = target.position + currentRotation * (offset.normalized * currentDistance + Vector3.up * height);
            transform.position = desiredPosition;

            // make the camera look at the planet
            transform.LookAt(target);
        }

    }

    public void SetCenteredObject(GameObject newTarget)
    {
        //GameObject newTarget = GameObject.Find(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(centerCameraOnInputField.text));
        if (newTarget != null && !newTarget.Equals(target.gameObject))
        {
            target = newTarget.transform;
            float dist = Vector3.Distance(transform.position, newTarget.transform.position);
            lerpTime = Mathf.Sqrt(dist / 3000f);
            doLerp = true;
            lastCameraPositionBeforeLerp = transform.position;
            lastCameraRotationBeforeLerp = transform.rotation;
            currentLerpTime = 0f;
            lerpTimeScale = 1f;
        }
    }
}
