using UnityEngine;

public class SelectionIndictator : MonoBehaviour
{
    private LineRenderer[] lineRenderers;
    private Material arcMaterial;

    private GravityObject target;
    private GravityObjectInfoPanel gravityObjectInfoPanel;

    private float currentIndicatorRotationDegrees = 0f;

    // public float radius = 5f;

    [Range(1, 6)]
    public int numArcs = 4;

    [Range(0, 360)]
    public int arcDegrees = 45;

    private void Awake()
    {
        lineRenderers = new LineRenderer[numArcs];
        // radius = Mathf.Abs(radius);
        arcMaterial = Resources.Load<Material>("Materials/SelectionArc");
        gravityObjectInfoPanel = FindObjectOfType<GravityObjectInfoPanel>();
    }

    private void Start()
    {
        for (int i = 0; i < numArcs; i++)
        {
            GameObject o = new GameObject("Selection Arc");
            o.transform.parent = transform;
            lineRenderers[i] = o.AddComponent<LineRenderer>();
        }

        for (int i = 0; i < numArcs; i++)
        {

            float centerAngle = 360f * i / numArcs;
            int numSegmentsPerArc = arcDegrees / 2;

            Vector3[] points = new Vector3[numSegmentsPerArc];

            for (int j = 0; j < numSegmentsPerArc; j++)
            {
                float ang = Mathf.Lerp(centerAngle - arcDegrees / 2f, centerAngle + arcDegrees / 2f, j * 1f / numSegmentsPerArc);
                float rad = Mathf.Deg2Rad * ang;
                //points[j] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
                points[j] = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            }

            lineRenderers[i].positionCount = numSegmentsPerArc;
            lineRenderers[i].SetPositions(points);
            lineRenderers[i].material = arcMaterial;
            lineRenderers[i].useWorldSpace = false;
            lineRenderers[i].transform.localScale = Vector3.one;
            lineRenderers[i].widthMultiplier = 2f;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 offset = Camera.main.transform.position - transform.position;

            transform.position = target.transform.position;
            foreach (var lineRenderer in lineRenderers)
            {
                lineRenderer.widthMultiplier = offset.magnitude / 100f;
            }

            Quaternion rotToLookAtTarget = Quaternion.LookRotation(offset.normalized);
            rotToLookAtTarget *= Quaternion.AngleAxis(currentIndicatorRotationDegrees, Vector3.forward);
            transform.rotation = rotToLookAtTarget;

            currentIndicatorRotationDegrees = (currentIndicatorRotationDegrees + Time.deltaTime * 12f) % 360f;
        }
    }

    public void SelectGravityObject(GravityObject go)
    {
        if (go == null)
            return;

        transform.localScale = Vector3.one * (go.GetCurrentRadius() * .65f + 3);
        // transform.position = go.transform.position;
        target = go;

        gravityObjectInfoPanel.SetTargetGravityObject(go);
    }
}
