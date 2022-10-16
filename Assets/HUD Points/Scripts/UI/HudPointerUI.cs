using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudPointerUI : HudPointUIBase
{
    #region SERIALIZE FIELDS
    [SerializeField] private GameObject rootUI;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI distanceText;
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
        if(distanceText)
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
        return distanceActive;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clampDirection"></param>
    public override void SetOffScreen(float clampDirection)
    {

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
