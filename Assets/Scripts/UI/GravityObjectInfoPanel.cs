using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravityObjectInfoPanel : MonoBehaviour
{
    private GravityObject target;
    private TextMeshProUGUI gravityObjectName, gravityObjectInfo, infoPanelShowHideButtonIconText;
    private Button infoPanelShowHideButton;
    private bool infoPanelVisible = false;
    private RectTransform rectTransform;

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();

        gravityObjectName = rectTransform.Find("Gravity Object Name").GetComponent<TextMeshProUGUI>();
        gravityObjectInfo = rectTransform.Find("Gravity Object Info").GetComponent<TextMeshProUGUI>();
        infoPanelShowHideButton = rectTransform.Find("Show Hide Panel").GetComponent<Button>();
        infoPanelShowHideButton.onClick.AddListener(delegate { OnToggleInfoPanelVisible(); });
        infoPanelShowHideButtonIconText = infoPanelShowHideButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateInfoPanelBasedOnVisibility();
    }

    public void OnToggleInfoPanelVisible()
    {
        infoPanelVisible = !infoPanelVisible;
        UpdateInfoPanelBasedOnVisibility();
    }

    private void UpdateInfoPanelBasedOnVisibility()
    {
        if (infoPanelVisible)
        {
            rectTransform.anchoredPosition = Vector3.zero;
            infoPanelShowHideButtonIconText.text = ">";
        }
        else
        {
            rectTransform.anchoredPosition = new Vector3(200, 0, 0);
            infoPanelShowHideButtonIconText.text = "<";
        }
    }

    public void SetTargetGravityObject(GravityObject obj)
    {
        target = obj;
        gravityObjectName.text = obj.name;
        gravityObjectInfo.text = GetTextDescriptionFromPlanetInfo(obj.info, obj.GetType() == typeof(CentralGravityObject));
    }

    private string GetTextDescriptionFromPlanetInfo(PlanetInitializer.PlanetInfo planetInfo, bool isCentralGravityObject = false)
    {
        if (isCentralGravityObject)
            return string.Join("\n",
                $"Mass: {planetInfo.Mass.ToString("E2")}kg",
                $"Radius: {planetInfo.Radius.ToString("F1")}km",
                $"Rotation period: {planetInfo.RotationPeriod} hours");

        string result = string.Join("\n",
            $"Mass: {planetInfo.Mass.ToString("E2")}kg",
            $"Planet radius: {planetInfo.Radius.ToString("F1")}km",
            $"Orbital radius: {planetInfo.InitialDisplacement.ToString("E2")}km",
            $"Orbital velocity: {planetInfo.InitialVelocity.ToString("F1")}km/s",
            $"Orbital angle: {planetInfo.InitialInclination} degrees",
            $"Length of day: {planetInfo.RotationPeriod} hours");

        return result;
    }
}
