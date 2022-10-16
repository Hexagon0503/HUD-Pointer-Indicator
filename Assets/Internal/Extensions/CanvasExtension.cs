using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class CanvasExtensions
{
    private static Dictionary<Canvas, RectTransform> runtimeCanvas = new Dictionary<Canvas, RectTransform>();

    /// <summary>
    /// Converts a world space position into a canvas space position
    /// </summary>
    /// <param name="Canvas"></param>
    /// <param name="World Position"></param>
    /// <param name="Current Camera"></param>
    /// <returns></returns>
    public static Vector3 WorldToCanvas(this Canvas canvas, Vector3 world_position, Camera camera)
    {
        if (camera == null)
        {
            return Vector3.zero;
        }
        var viewport_position = camera.WorldToViewportPoint(world_position);
        var canvas_rect = canvas.GetComponent<RectTransform>();
        return new Vector3((viewport_position.x * canvas_rect.sizeDelta.x) - (canvas_rect.sizeDelta.x * 0.5f), (viewport_position.y * canvas_rect.sizeDelta.y) - (canvas_rect.sizeDelta.y * 0.5f), viewport_position.z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="anchoredPosition"></param>
    /// <returns></returns>
    public static bool IsVisible(this Canvas canvas, Vector3 canvasPos)
    {
        if(canvasPos.z < 0)
        {
            return false;
        }
        var canvas_rect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvas_rect.sizeDelta.x / 2;
        float canvasHeight = canvas_rect.sizeDelta.x / 2;
        if (canvasPos.x < -canvasWidth || canvasPos.x > canvasWidth || canvasPos.y < -canvasHeight || canvasPos.y > canvasHeight)
        {
            return false;
        }
        return true;
    }
}