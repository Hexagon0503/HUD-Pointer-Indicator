using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class GameCanvas : MonoBehaviour
{
    #region FUNCTIONS
    /// <summary>
    /// Converts a world space position into a canvas space position
    /// </summary>
    /// <param name="Canvas"></param>
    /// <param name="World Position"></param>
    /// <param name="Current Camera"></param>
    /// <returns></returns>
    private Vector3 viewport_position;
    public Vector3 WorldToCanvas(Vector3 world_position, Camera camera)
    {
        if (camera == null)
        {
            return Vector3.zero;
        }
        viewport_position = camera.WorldToViewportPoint(world_position);
        viewport_position.x = (viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f);
        viewport_position.y = (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f);
        return viewport_position;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="anchoredPosition"></param>
    /// <returns></returns>
    public bool IsVisible(Vector3 canvasPos, Vector2 sizeDelta, float boundScale = 1)
    {
        if (canvasPos.z < 0)
        {
            return false;
        }
        GetCanvasHalfSize(boundScale);
        if (canvasPos.x < (-canvasHalfSize.x + sizeDelta.x * 0.5f) || canvasPos.x > (canvasHalfSize.x - sizeDelta.x * 0.5f) || canvasPos.y < (-canvasHalfSize.y + sizeDelta.y * 0.5f) || (canvasPos.y > canvasHalfSize.y - sizeDelta.y * 0.5f))
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Vector2 canvasHalfSize;
    public Vector2 GetCanvasHalfSize(float boundScale = 1)
    {
        canvasHalfSize = canvas_rect.sizeDelta * boundScale * 0.5f;
        return canvasHalfSize;
    }
    #endregion

    #region GETTER
    /// <summary>
    /// 
    /// </summary>
    private Canvas _canvas;
    private Canvas canvas
    {
        get
        {
            if(_canvas == null)
            {
                _canvas = GetComponent<Canvas>();
            }
            return _canvas;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private RectTransform _rectTransform;
    public RectTransform canvas_rect
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = canvas.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }
    #endregion
}
