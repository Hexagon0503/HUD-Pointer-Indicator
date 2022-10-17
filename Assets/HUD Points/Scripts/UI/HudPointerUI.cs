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
    private float lastDistance = 0;
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
        rootUI.SetActive(active && distanceActive);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="distance"></param>
    public override bool SetDistance(float distance)
    {
        if (lastDistance != distance)
        {
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
    /// <param name="clampDirection"></param>
    private Vector3 arrowRotation;
    public override void SetOffScreen(float clampDirection)
    {
        showOnOffScreen?.SetActiveOptimized(clampDirection != -1);
        hideOnOffScreen?.SetActiveOptimized(clampDirection == -1);
        if (clampDirection != -1)
        {
            arrowRotation.z = clampDirection;
            offScreenArrow.localRotation = Quaternion.Euler(arrowRotation);
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
        distanceActive = false;
        iconImage.sprite = null;
        distanceText.gameObject.SetActive(false);
    }
    #endregion
}
