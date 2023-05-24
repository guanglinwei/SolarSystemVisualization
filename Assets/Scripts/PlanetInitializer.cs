using UnityEngine;

// TODO:
// increase accuracy of orbital lines by updating using a path after the planet completes a full rotation
// make input event handled by one separate program

public class PlanetInitializer : MonoBehaviour
{
    private const float gravConstant = 6.67408e-10f;
    private Material orbitLineMaterial;

    public struct PlanetInfo
    {
        public PlanetInfo(string name, float mass, float radius, float initialDisplacement, float initialVelocity, float rotationPeriod)
        {
            Name = name;
            Mass = mass;
            Radius = radius * planetScale;
            InitialDisplacement = initialDisplacement;
            InitialVelocity = initialVelocity * velocityScale;
            RotationSpeed = 1f / (rotationPeriod * 10f); // 360 deg * period (hrs) / 3600 (sec / hr)
            //InitialVelocity = Mathf.Sqrt(gravConstant * Mass / InitialDisplacement);
        }

        public string Name;
        public float Mass;
        public float Radius;
        public float InitialDisplacement; // Distance from Sun to planet in 1e6 km
        public float InitialVelocity;
        public float RotationSpeed;
    }

    [HideInInspector]
    public PlanetInfo[] planets =
    {
        //              Name        Mass    Radius  InitialDisplacement  InitialVelocity  RotationPeriod
        new PlanetInfo("Mercury", 0.330e-4f,  2440,         57.9f,              47.4f,       1407.6f),
        new PlanetInfo("Venus",   4.870e-4f,  6052,        108.2f,              35f,        -5832.5f),
        new PlanetInfo("Earth",   5.970e-4f,  6371,        149.6f,              29.8f,         23.9f),
        new PlanetInfo("Mars",    0.642e-4f,  3390,          228f,              24.1f,         24.6f),
        new PlanetInfo("Jupiter",  1898e-4f, 69911,        778.5f,              13.1f,          9.9f),
        new PlanetInfo("Saturn",    568e-4f, 58232,         1432f,              9.7f,          10.7f),
        new PlanetInfo("Uranus",   86.8e-4f, 25362,         2867f,              6.8f,         -17.2f),
        new PlanetInfo("Neptune",   102e-4f, 24622,         4515f,              5.4f,          16.1f),
        //new PlanetInfo("Pluto", 0, 0, 5900),

        // 365.2 days 
        // v*t = 2pir
    };

    public GameObject planetPrefab;
    public GameObject centralPlanetPrefab;

    // https://nssdc.gsfc.nasa.gov/planetary/factsheet/
    public const float planetScale = 10f/6371; // Earth's radius scaled from 6371km to 10 units
    public const float velocityScale = 1e-6f; // Velocity scaled from km/s to units/s
    //private static float massScale = 1e-28f;
    public const float distanceScale = 1e-6f;  // Distance from Earth to Sun scaled from 1.5e8km to 150 units.
    public const float sunScale = 0.07f; // Sun must be smaller to prevent other planets from overlapping it given the smaller scale

    private void SetPlanetTexture(GameObject planet, string name)
    {
        //Texture2D tex = Resources.Load<Texture2D>("Assets/Textures/" + texPath);
        //planet.GetComponent<Renderer>().material.mainTexture = tex;
        planet.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/" + name);
    }
    public void InitPlanets()
    {
        GameObject simulationParent = GameObject.Find("Gravity Objects");
        if (simulationParent == null)
        {
            simulationParent = new GameObject("Gravity Objects");
        }

        GameObject sun = GameObject.Find("Sun");
        if (sun == null)
        {
            sun = Instantiate(centralPlanetPrefab, Vector3.zero, Quaternion.identity);
            //Light sunLight = sun.AddComponent<Light>();
            //sunLight.type = LightType.Point;
            //sunLight.range = 6000f;
            //sunLight.color = new Color(1.0f, .9f, .8f);
            sun.name = "Sun";
        }

        GravityObject sunGravityObject = sun.GetComponent<CentralGravityObject>();
        if (sunGravityObject == null)
        {
            sunGravityObject = sun.AddComponent<CentralGravityObject>();
        }

        // sunGravityObject.mass = 1989100e6f;
        sunGravityObject.mass = 198.9f;
        sunGravityObject.radius = 695700 * planetScale * sunScale;
        sunGravityObject.realScaleRadius = sunGravityObject.radius;
        sunGravityObject.rotationSpeed = 1f / (27 * 10 * 24);
        SetPlanetTexture(sun, "Sun");

        sun.transform.parent = simulationParent.transform;
        FindObjectOfType<LockedCameraController>().target = sun.transform;
        orbitLineMaterial = Resources.Load<Material>("Materials/OrbitLine");

        foreach (PlanetInfo planet in planets)
        {
            GameObject o = GameObject.Find(planet.Name);
            if (o == null)
            {
                Debug.Log("adding " + planet.Name);
                o = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
                o.name = planet.Name;
            }

            o.transform.position = Vector3.right * planet.InitialDisplacement;
            o.transform.parent = simulationParent.transform;

            GravityObject gravityObject = o.GetComponent<GravityObject>();
            if (gravityObject == null)
            {
                gravityObject = o.AddComponent<GravityObject>();
            }

            gravityObject.mass = planet.Mass;
            gravityObject.radius = planet.Radius;
            gravityObject.realScaleRadius = planet.Radius / planetScale * distanceScale;
            gravityObject.initialVelocity = planet.InitialVelocity * new Vector3(0, 0, 1);
            gravityObject.rotationSpeed = planet.RotationSpeed;
            SetPlanetTexture(o, planet.Name);

            // Create orbital path
            // GameObject orbitLineRendererGameObject = transform.Find(planet.Name + "Orbit")?.gameObject;
            GameObject orbitLineRendererGameObject = GameObject.Find(planet.Name + "Orbit");
            if (orbitLineRendererGameObject == null)
            {
                orbitLineRendererGameObject = new GameObject(planet.Name + "Orbit");
                orbitLineRendererGameObject.transform.parent = sun.transform;
                    
            }
            LineRenderer orbitLineRenderer = orbitLineRendererGameObject.GetComponent<LineRenderer>();
            if (orbitLineRenderer == null)
            {
                orbitLineRenderer = orbitLineRendererGameObject.AddComponent<LineRenderer>();
            }

            const int segments = 360;
            orbitLineRenderer.useWorldSpace = false;
            orbitLineRenderer.positionCount = segments;
            orbitLineRenderer.loop = true;
            orbitLineRendererGameObject.transform.localScale = Vector3.one / sunGravityObject.radius;

            Vector3[] points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float rad = Mathf.Deg2Rad * i * 360f / segments;
                points[i] = new Vector3(Mathf.Sin(rad) * planet.InitialDisplacement, 0, Mathf.Cos(rad) * planet.InitialDisplacement);
            }

            orbitLineRenderer.SetPositions(points);
            orbitLineRenderer.material = orbitLineMaterial;
        }
    }
}
