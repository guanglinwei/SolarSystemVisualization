using UnityEngine;

// TODO:
// increase accuracy of orbital lines by updating using a path after the planet completes a full rotation
// make input event handled by one separate program

public class PlanetInitializer : MonoBehaviour
{
    private const float gravConstant = 6.67408e-10f;
    private Material orbitLineMaterial;

    [System.Serializable]
    public struct PlanetInfo
    {
        /// <summary>
        /// Constructor for struct that holds info on planet/gravity object
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="mass">kg</param>
        /// <param name="radius">km</param>
        /// <param name="initialDisplacement">km</param>
        /// <param name="initialVelocity">km/s</param>
        /// <param name="initialInclination">degs</param>
        /// <param name="rotationPeriod">hrs</param>
        /// <param name="obliquity">degs</param>
        public PlanetInfo(string name, float mass, float radius, float initialDisplacement, float initialVelocity, float initialInclination, float rotationPeriod, float obliquity)
        {
            Name = name;
            Mass = mass;
            Radius = radius;
            InitialDisplacement = initialDisplacement;
            InitialVelocity = initialVelocity;
            InitialInclination = initialInclination;
            RotationPeriod = rotationPeriod;
            Obliquity = obliquity;
        }

        public string Name;
        public float Mass;
        public float Radius;
        public float InitialDisplacement; // Distance from Sun to planet in 1e6 km
        public float InitialVelocity;
        public float InitialInclination;
        //public float RotationSpeed;
        public float RotationPeriod;
        public float Obliquity;
        //public Vector3 RotationAxis;
    }

    // https://nssdc.gsfc.nasa.gov/planetary/factsheet/
    [HideInInspector]
    public static readonly PlanetInfo sunInfo = new PlanetInfo("Sun", 1.989e30f, 695700, 0f, 0f, 0f, 27 * 24, 0f);

    [HideInInspector]
    public static readonly PlanetInfo[] planetInfos =
    {
        //              Name        Mass    Radius  InitialDisplacement  InitialVelocity  InitialInclination  RotationPeriod  Obliquity
        new PlanetInfo("Mercury", 0.330e24f,  2440,         57.9e6f,              47.4f,            7f,             1407.6f,       .034f),
        new PlanetInfo("Venus",   4.870e24f,  6052,        108.2e6f,                35f,          3.4f,            -5832.5f,      177.4f),
        new PlanetInfo("Earth",   5.970e24f,  6371,        149.6e6f,              29.8f,            0f,               23.9f,       23.4f),
        new PlanetInfo("Mars",    0.642e24f,  3390,          228e6f,              24.1f,          1.8f,               24.6f,       25.2f),
        new PlanetInfo("Jupiter",  1898e24f, 69911,        778.5e6f,              13.1f,          1.3f,                9.9f,        3.1f),
        new PlanetInfo("Saturn",    568e24f, 58232,         1432e6f,               9.7f,          2.5f,               10.7f,       26.7f),
        new PlanetInfo("Uranus",   86.8e24f, 25362,         2867e6f,               6.8f,          0.8f,              -17.2f,       97.8f),
        new PlanetInfo("Neptune",   102e24f, 24622,         4515e6f,               5.4f,          1.8f,               16.1f,       28.3f),
        //new PlanetInfo("Pluto", 0, 0, 5900),
    };

    public const float planetScale = 10f/6371; // Earth's diameter scaled from 6371km to 10 units
    public const float velocityScale = 1e-6f; // Velocity scaled from km/s to units/s
    public const float massScale = 1e-28f; // 1e24 kg to 1e-4kg
    public const float distanceScale = 1e-6f;  // Distance from Earth to Sun scaled from 1.5e8km to 150 units.
    public const float sunScale = 0.07f; // Sun must be smaller to prevent other planets from overlapping it given the smaller scale

    public GameObject planetPrefab;
    public GameObject centralPlanetPrefab;
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

        // Load the sun
        GameObject sun = GameObject.Find("Sun");
        if (sun == null)
        {
            sun = Instantiate(centralPlanetPrefab, Vector3.zero, Quaternion.identity);
            //Light sunLight = sun.AddComponent<Light>();
            //sunLight.type = LightType.Point;
            //sunLight.range = 6000f;
            //sunLight.color = new Color(1.0f, .9f, .8f);
            sun.name = sunInfo.Name;
            sun.layer = LayerMask.NameToLayer("GravityObjects");
        }

        GravityObject sunGravityObject = sun.GetComponent<CentralGravityObject>();
        if (sunGravityObject == null)
        {
            sunGravityObject = sun.AddComponent<CentralGravityObject>();
        }

        sunGravityObject.mass = sunInfo.Mass * massScale;
        sunGravityObject.radius = sunInfo.Radius * planetScale * sunScale;
        sunGravityObject.realScaleRadius = sunInfo.Radius;
        sunGravityObject.rotationSpeed = 1f / (10f * sunInfo.RotationPeriod);
        sunGravityObject.info = sunInfo;
        SetPlanetTexture(sun, "Sun");

        sun.transform.parent = simulationParent.transform;
        FindObjectOfType<LockedCameraController>().target = sun.transform;
        orbitLineMaterial = Resources.Load<Material>("Materials/OrbitLine");

        // Load the planets
        foreach (PlanetInfo planet in planetInfos)
        {
            GameObject o = GameObject.Find(planet.Name);
            if (o == null)
            {
                Debug.Log("adding " + planet.Name);
                o = Instantiate(planetPrefab, Vector3.zero, Quaternion.identity);
                o.name = planet.Name;
                o.layer = LayerMask.NameToLayer("GravityObjects");
            }



            o.transform.position = Quaternion.Euler(0f, 0f, planet.InitialInclination) * Vector3.right * planet.InitialDisplacement * distanceScale;
            o.transform.rotation = Quaternion.Euler(0f, 0f, planet.InitialInclination + planet.Obliquity);
            o.transform.parent = simulationParent.transform;

            GravityObject gravityObject = o.GetComponent<GravityObject>();
            if (gravityObject == null)
            {
                gravityObject = o.AddComponent<GravityObject>();
            }

            gravityObject.mass = planet.Mass * massScale;
            gravityObject.radius = planet.Radius * planetScale;
            gravityObject.realScaleRadius = planet.Radius * distanceScale;
            gravityObject.initialVelocity = planet.InitialVelocity * velocityScale * new Vector3(0, 0, 1);
            gravityObject.rotationSpeed = 1f / (planet.RotationPeriod * 10f);
            gravityObject.rotationAxis = new Vector3(0f, planet.InitialInclination + planet.Obliquity, 0f);
            gravityObject.info = planet;
            SetPlanetTexture(o, planet.Name);

            // Create orbital path
            // GameObject orbitLineRendererGameObject = transform.Find(planet.Name + "Orbit")?.gameObject;
            GameObject orbitLineRendererContainerGameObject = GameObject.Find("Orbit Container");
            if (orbitLineRendererContainerGameObject == null)
            {
                orbitLineRendererContainerGameObject = new GameObject("Orbit Container");
            }
            orbitLineRendererContainerGameObject.transform.parent = sun.transform;
            orbitLineRendererContainerGameObject.transform.localScale = Vector3.one / sunGravityObject.radius;

            GameObject orbitLineRendererGameObject = GameObject.Find(planet.Name + "Orbit");
            if (orbitLineRendererGameObject == null)
            {
                orbitLineRendererGameObject = new GameObject(planet.Name + "Orbit");

            }
            orbitLineRendererGameObject.transform.parent = orbitLineRendererContainerGameObject.transform;

            LineRenderer orbitLineRenderer = orbitLineRendererGameObject.GetComponent<LineRenderer>();
            if (orbitLineRenderer == null)
            {
                orbitLineRenderer = orbitLineRendererGameObject.AddComponent<LineRenderer>();
            }

            const int segments = 360;
            orbitLineRenderer.useWorldSpace = false;
            orbitLineRenderer.positionCount = segments;
            orbitLineRenderer.loop = true;
            orbitLineRenderer.transform.localScale = Vector3.one;

            Vector3[] points = new Vector3[segments];
            for (int i = 0; i < segments; i++)
            {
                float rad = Mathf.Deg2Rad * i * 360f / segments;
                points[i] = Quaternion.Euler(0f, 0f, planet.InitialInclination) * 
                    new Vector3(Mathf.Sin(rad) * planet.InitialDisplacement, 0, Mathf.Cos(rad) * planet.InitialDisplacement) * distanceScale;
            }

            orbitLineRenderer.SetPositions(points);
            orbitLineRenderer.material = orbitLineMaterial;
        }
    }
}
