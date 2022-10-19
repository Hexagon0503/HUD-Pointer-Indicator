using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudPointerUI : HudPointUIBase
{
    #region SERIALIZE FIELDS
    [SerializeField] private GameObject rootUI;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI distanceText;
    [Header("Off Screen")]
    [SerializeField] private GameObject showOnOffScreen;
    [SerializeField] private GameObject hideOnOffScreen;
    [SerializeField] private RectTransform offScreenArrow;
    [Header("Scale by Distance")]
    [SerializeField] private bool scaleByDistance = false;
    [SerializeField] private Vector2 minMaxDistance = new Vector2(7.5f, 15f);
    [SerializeReference] private float minScale = 0.75f;
    #endregion

    #region FIELDS
    /// <summary>
    /// 
    /// </summary>
    private HudPointData hudData = null;

    /// <summary>
    /// 
    /// </summary>
    private bool distanceActive = true;

    /// <summary>
    /// 
    /// </summary>
    private float distanceScalePercent;

    /// <summary>
    /// 
    /// </summary>
    private float lastDistance = 0;

    /// <summary>
    /// 
    /// </summary>
    private bool onOffScreen;

    /// <summary>
    /// 
    /// </summary>
    private Vector3 arrowRotation;
    #endregion

    #region METHODS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public override void SetInfo(BaseHudPointerData data)
    {
        distanceActive = true;
        if (data is HudPointData)
        {
            hudData = (data as HudPointData);
            if (iconImage)
            {
                iconImage.sprite = hudData.iconSprite;
            }
        }
        distanceText.gameObject.SetActive(data.CheckDistance);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public override void SetActive(bool active)
    {
        rootUI.SetActiveOptimized(active && distanceActive);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="distance"></param>
    public override bool SetDistance(float distance)
    {
        if (lastDistance != distance)
        {
            if (scaleByDistance)
            {
                distanceScalePercent = (distance - minMaxDistance.x) / (minMaxDistance.y - minMaxDistance.x);
                rectTransform.localScale = Vector3.one * Mathf.Lerp(minScale, 1, 1 - distanceScalePercent);
            }
            if (distanceText)
            {
                distanceText.text = $"{distance.ToString("F0")}m";
            }
            if (hudData != null)
            {
                if (hudData.hideInCloseDistance && distance <= hudData.hideCloseDistance || hudData.hideInFarDistance && distance >= hudData.hideFarDistance)
                {
                    distanceActive = false;
                }
                else
                {
                    distanceActive = true;
                }
            }
            lastDistance = distance;
        }
        return distanceActive;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    /// <param name="angle"></param>
    public override void SetOffScreen(bool active, float angle = 0)
    {
        if (onOffScreen != active)
        {
            showOnOffScreen?.SetActiveOptimized(active);
            hideOnOffScreen?.SetActiveOptimized(!active);
            if (active)
            {
                arrowRotation.z = angle;
                offScreenArrow.localRotation = Quaternion.Euler(arrowRotation);
            }
            onOffScreen = active;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public override void UpdateInfo(object data) { }

    /// <summary>
    /// 
    /// </summary>
    public override void ResetUI()
    {
        hudData = null;
        iconImage.sprite = null;
        distanceActive = false;
        onOffScreen = false;
        lastDistance = -1;
        distanceText.gameObject.SetActive(false);
    }
    #endregion
}
