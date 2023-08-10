using UnityEngine;
using UnityEngine.EventSystems;
// TODO:
// make it so you cant zoom into interior of a planet
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
    private Vector3 currentRotationEulerAngles;

    private bool doLerp = false;
    private float lerpTime = 1f;
    private float currentLerpTime = 0f;
    private float lerpTimeScale = 1f;
    private Vector3 lastCameraPositionBeforeLerp;
    private Quaternion lastCameraRotationBeforeLerp;

    private Vector3 lastMouseDownMousePos = -Vector3.one;
    private bool isRotatingWithMouse = false;

    private SelectionIndictator selectionIndictator;

    [HideInInspector]
    public bool isRealPlanetScale = false;

    private void Start()
    {
        offset = transform.position - target.position;
        currentDistance = distance;
        currentRotation = transform.rotation;
        currentRotationEulerAngles = currentRotation.eulerAngles;

        selectionIndictator = FindObjectOfType<SelectionIndictator>();
    }

    private void LateUpdate()
    {
        // calculate the desired position of the camera
        Vector3 desiredPosition = target.position + currentRotation * (Vector3.back * currentDistance + Vector3.up * height);

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
            bool mouseIsAboveUI = EventSystem.current.IsPointerOverGameObject();

            // handle mouse input for selecting gravity objects
            if (!mouseIsAboveUI && Input.GetKeyDown(KeyCode.Mouse0))
            {
                isRotatingWithMouse = true;
                lastMouseDownMousePos = Input.mousePosition;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isRotatingWithMouse = false;

                // if mouse didn't move in between mousedown and mouseup
                if (Input.mousePosition == lastMouseDownMousePos)
                {
                    SelectGravityObjectAtMousePos();
                }

                lastMouseDownMousePos = -Vector3.one;
            }

            // handle mouse input for camera rotation
            if (isRotatingWithMouse && Input.GetKey(KeyCode.Mouse0))
            {
                float rotateHorizontal = Input.GetAxis("Mouse X") * rotateSpeed;
                float rotateVertical = Input.GetAxis("Mouse Y") * rotateSpeed;
                // currentRotation *= Quaternion.Euler(-rotateVertical, rotateHorizontal, 0f);
                // currentRotationEulerAngles += new Vector3(-rotateVertical, rotateHorizontal, 0f);
                currentRotationEulerAngles.y += rotateHorizontal;

                float newRotX = currentRotationEulerAngles.x - rotateVertical;

                if (currentRotationEulerAngles.x > 80f && currentRotationEulerAngles.x <= 90f && newRotX > 90f)
                    newRotX = 89f;
                else if (currentRotationEulerAngles.x < 280f && currentRotationEulerAngles.x >= 270f && newRotX < 270f)
                    newRotX = 271f;

                currentRotationEulerAngles.x = newRotX;

                currentRotation = Quaternion.Euler(currentRotationEulerAngles);
                currentRotationEulerAngles = currentRotation.eulerAngles;
            }

            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            float currentZoomSpeed = Mathf.Max(isRealPlanetScale ? 0.01f : 1f, Mathf.Sqrt(currentDistance / 160f)) * (Input.GetKey(KeyCode.LeftControl) ? fastZoomSpeed : zoomSpeed);
            currentDistance = Mathf.Clamp(currentDistance - mouseScroll * currentZoomSpeed, 0.1f, 4700f);

            // calculate the desired position of the camera
            desiredPosition = target.position + currentRotation * (Vector3.back * currentDistance + Vector3.up * height);
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

    public void SelectGravityObjectAtMousePos()
    {
        GravityObject go = GetGravityObjectAtMousePos();
        selectionIndictator.SelectGravityObject(go);
    }

    public GravityObject GetGravityObjectAtMousePos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 6000f, LayerMask.GetMask("GravityObjects")))
        {
            GameObject obj = hit.collider.gameObject;
            GravityObject go;
            obj.TryGetComponent(out go);

            return go;
        }

        return null;
    }
}
