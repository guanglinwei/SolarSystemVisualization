using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GravityObjectInfoPanel : MonoBehaviour
{
    private GravityObject target;
    private TextMeshProUGUI gravityObjectName, gravityObjectInfo, showHideButtonIconText;
    private Button showHideButton, nextPageButton, prevPageButton;
    private bool infoPanelVisible = true;
    private RectTransform rectTransform;

    private int currentPage = 1;

    private static readonly Dictionary<string, string> gravityObjectDescriptions = new Dictionary<string, string>
    {
        { "Sun", "desc" },
        {"Mercury", "desc"},
        {"Venus", "desc"},
        {"Earth", "desc"},
        {"Mars", "desc"},
        {"Jupiter", "desc"},
        {"Saturn", "desc"},
        {"Uranus", "desc"},
        {"Neptune", "desc"},
    };

    public void Init()
    {
        rectTransform = GetComponent<RectTransform>();

        gravityObjectName = rectTransform.Find("Gravity Object Name").GetComponent<TextMeshProUGUI>();
        gravityObjectInfo = rectTransform.Find("Gravity Object Info").GetComponent<TextMeshProUGUI>();

        showHideButton = rectTransform.Find("Show Hide Button").GetComponent<Button>();
        showHideButton.onClick.AddListener(delegate { OnToggleInfoPanelVisible(); });
        showHideButtonIconText = showHideButton.GetComponentInChildren<TextMeshProUGUI>();

        nextPageButton = rectTransform.Find("Next Page").GetComponent<Button>();
        nextPageButton.onClick.AddListener(delegate { GoToPage(currentPage + 1); });
        prevPageButton = rectTransform.Find("Previous Page").GetComponent<Button>();
        prevPageButton.onClick.AddListener(delegate { GoToPage(currentPage - 1); });

        UpdateInfoPanelBasedOnVisibility();
        gameObject.SetActive(false);
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
            showHideButtonIconText.text = ">";
        }
        else
        {
            rectTransform.anchoredPosition = new Vector3(rectTransform.rect.width, 0, 0);
            showHideButtonIconText.text = "<";
        }
    }

    public void SetTargetGravityObject(GravityObject obj)
    {
        target = obj;

        if (obj == null)
        {
            gameObject.SetActive(false);
            UpdateInfoPanelBasedOnVisibility();
        }
        else
        {
            gameObject.SetActive(true);
            gravityObjectName.text = obj.name;
            gravityObjectInfo.text = GetTextDescriptionFromPlanetInfo(obj.info, obj.GetType() == typeof(CentralGravityObject));
            GoToPage(1);
        }
    }

    private string GetTextDescriptionFromPlanetInfo(PlanetInitializer.PlanetInfo planetInfo, bool isCentralGravityObject = false)
    {
        if (isCentralGravityObject)
            return string.Join("\n",
                $"Mass: {planetInfo.Mass.ToString("E2")}kg",
                $"Radius: {planetInfo.Radius.ToString("F1")}km",
                $"Rotation period: {planetInfo.RotationPeriod} hrs");

        string result = string.Join("\n",
            $"Mass: {planetInfo.Mass.ToString("E2")}kg",
            $"Planet radius: {planetInfo.Radius.ToString("F1")}km",
            $"Orbital radius: {planetInfo.InitialDisplacement.ToString("E2")}km",
            $"Orbital velocity: {planetInfo.InitialVelocity.ToString("F1")}km/s",
            $"Orbital angle: {planetInfo.InitialInclination}°",
            $"Length of day: {planetInfo.RotationPeriod} hrs",

            "<page>" + gravityObjectDescriptions[planetInfo.Name]);

        return result;
    }

    private void GoToPage(int page)
    {
        page = Mathf.Clamp(page, 1, Mathf.Max(1, gravityObjectInfo.textInfo.pageCount));
        currentPage = page;
        gravityObjectInfo.pageToDisplay = page;
        prevPageButton.interactable = false;
        nextPageButton.interactable = false;

        StartCoroutine(UpdatePageButtonsInteractable(page));
    }

    // Must wait a frame before updating buttons because
    // TextMeshPro does not update textInfo until the frame after changing the text
    private IEnumerator UpdatePageButtonsInteractable(int page)
    {
        yield return 0;

        prevPageButton.interactable = page > 1;
        nextPageButton.interactable = page < gravityObjectInfo.textInfo.pageCount;
    }
}
