using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInputController : MonoBehaviour
{
    public TMP_InputField centerCameraOnInputField;
    private LockedCameraController cameraController;

    public Toggle toggleOrbitalLinesField;
    private CentralGravityObject centralGravityObject;

    public Toggle toggleRealPlanetScaleField;
    private SimulationController simulationController;

    private GravityObjectInfoPanel infoPanel;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<LockedCameraController>();
        centerCameraOnInputField.onValueChanged.AddListener(delegate { OnChangeCenterCameraInputField(); });

        centralGravityObject = FindObjectOfType<CentralGravityObject>();
        toggleOrbitalLinesField.onValueChanged.AddListener(delegate { OnToggleOrbitalLinesField(); });

        simulationController = FindObjectOfType<SimulationController>();
        toggleRealPlanetScaleField.onValueChanged.AddListener(delegate { OnToggleRealPlanetScaleField(); });

        infoPanel = FindObjectOfType<GravityObjectInfoPanel>();
    }

    private void Start()
    {
        infoPanel.Init();
    }

    public void OnChangeCenterCameraInputField()
    {
        GameObject o = GameObject.Find(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(centerCameraOnInputField.text));
        cameraController.SetCenteredObject(o);
    }

    public void OnToggleOrbitalLinesField()
    {
        centralGravityObject.SetChildOrbitLinesVisible(toggleOrbitalLinesField.isOn);
    }

    public void OnToggleRealPlanetScaleField()
    {
        cameraController.isRealPlanetScale = toggleRealPlanetScaleField.isOn;
        simulationController.SetIsRealScale(toggleRealPlanetScaleField.isOn);
    }
}
