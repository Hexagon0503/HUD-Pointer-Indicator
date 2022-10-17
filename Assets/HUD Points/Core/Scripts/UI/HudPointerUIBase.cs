using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HudPointUIBase : MonoBehaviour
{
    #region METHODS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public abstract void SetActive(bool active);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public abstract void SetInfo(BaseHudPointerData data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public abstract void UpdateInfo(object data);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public abstract bool SetDistance(float distance);

    /// <summary>
    /// 
    /// </summary>
    public abstract void ResetUI();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public abstract void SetOffScreen(bool active, float angle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newAnchoredPos"></param>
    public virtual void SetAnchoredPosition(Vector3 newAnchoredPos)
    {
        rectTransform.anchoredPosition3D = newAnchoredPos;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="alpha"></param>
    public virtual void SetAlpha(float alpha)
    {
        if (!alphaGroup) return;
        //
        alphaGroup.alpha = alpha;
    }
    #endregion

    #region GETTERS
    /// <summary>
    /// 
    /// </summary>
    private CanvasGroup _alphaGroup = null;
    public CanvasGroup alphaGroup
    {
        get
        {
            if (_alphaGroup == null)
            {
                _alphaGroup = GetComponent<CanvasGroup>();
            }
            return _alphaGroup;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private RectTransform _rectTransform = null;
    public RectTransform rectTransform
    {
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    #endregion
}
