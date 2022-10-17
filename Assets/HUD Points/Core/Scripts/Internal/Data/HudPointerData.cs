using UnityEngine;

public class BaseHudPointerData
{
    [Header("Settings")]
    /// <summary>
    /// 
    /// </summary>
    public Transform target;
    /// <summary>
    /// 
    /// </summary>
    public GameObject pointerPrefab;

    /// <summary>
    /// 
    /// </summary>
    public bool CheckDistance;

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Offset;

    /// <summary>
    /// 
    /// </summary>
    public bool ClampPointer;

    /// <summary>
    /// 
    /// </summary>
    public object customData { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public HudPointUIBase runtimeUI { get; set; }
}


[System.Serializable]
public class HudPointData : BaseHudPointerData
{
    public Sprite iconSprite;
    [Header("Distance")]
    public bool hideInCloseDistance;
    public int hideCloseDistance = 15;
    public bool hideInFarDistance;
    public int hideFarDistance = 15;
}