using System.Collections.Generic;
using UnityEngine;

public class HudPointManager : MonoBehaviour
{
    #region SERIALZIE FIELDS
    [Header("Settings")]
    [SerializeField, Range(0.5f, 1f)] private float screenBoundScale = 1;
    [SerializeField] private bool useFrameDelay;
    [SerializeField, Range(5, 15)] private int updateFrameDelay = 5;
    [Space]
    [Header("References")]
    [SerializeField] private GameObject defaultHudPrefab;
    [SerializeField] private RectTransform rootPanel;
    [SerializeField] private GameCanvas uiCanvas;
    [SerializeField] private Camera gameCamera;
    #endregion

    #region FIELDS
    /// <summary>
    /// 
    /// </summary>
    private float _hudAlpha = 1;
    public float HudAlpha
    {
        get => _hudAlpha;
        set
        {
            _hudAlpha = value;
            UpdatePointerAlpha(value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<int, BaseHudPointerData> runtimePointers = new Dictionary<int, BaseHudPointerData>();

    /// <summary>
    /// 
    /// </summary>
    private int currentFrame;

    /// <summary>
    /// 
    /// </summary>
    int registerCount = 0;
    #endregion

    #region UNITY METHODS
    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        if (gameCamera == null || runtimePointers.Count < 1)
        {
            if (useFrameDelay)
                currentFrame = 0;
            return;
        }
        if (useFrameDelay)
        {
            if (currentFrame == 0)
            {
                UpdatePointers();
            }
            currentFrame = (currentFrame + 1) % updateFrameDelay;
        }
        else
        {
            UpdatePointers();
        }
    }
    #endregion

    #region REGISTER POINTS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int RegisterPoint(BaseHudPointerData data)
    {
        int id = registerCount;
        if(data.pointerPrefab == null)
        {
            data.pointerPrefab = defaultHudPrefab;
        }
        data.runtimeUI = Instantiate(data.pointerPrefab, rootPanel).GetComponent<HudPointUIBase>();
        data.runtimeUI.SetInfo(data);
        UpdateHudPointer(data, true);
        runtimePointers.Add(id, data);
        registerCount++;
        return id;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    public void RemovePoint(int ID)
    {
        if (ID == -1 || !runtimePointers.ContainsKey(ID)) return;
        //
        if (runtimePointers[ID].runtimeUI)
        {
            Destroy(runtimePointers[ID].runtimeUI.gameObject);
        }
        runtimePointers.Remove(ID);
    }
    #endregion

    #region UPDATE POINTER
    /// <summary>
    /// 
    /// </summary>
    public void UpdatePointers()
    {
        GetCanvasScale();
        foreach (BaseHudPointerData pointer in runtimePointers.Values)
        {
            UpdateHudPointer(pointer);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdatePointerAlpha(float newAlpha)
    {
        foreach (BaseHudPointerData pointer in runtimePointers.Values)
        {
            if (pointer.target == null || pointer.runtimeUI == null) return;
            //
            pointer.runtimeUI.SetAlpha(newAlpha);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointer"></param>
    private Vector3 canvasPos;
    private float targetDistance;
    private bool visibleInCanvas, pointerVisible;
    private Vector2 canvasSize, runtimeSize, minClamp, maxClamp;
    private void UpdateHudPointer(BaseHudPointerData pointer, bool forceUpdate = false)
    {
        if (pointer.target == null || pointer.runtimeUI == null) return;
        //
        if (forceUpdate)
        {
            pointer.runtimeUI.SetAlpha(HudAlpha);
            GetCanvasScale();
        }
        canvasPos = uiCanvas.WorldToCanvas(pointer.target.position + pointer.Offset, gameCamera);
        visibleInCanvas = uiCanvas.IsVisible(canvasPos, pointer.runtimeUI.rectTransform.sizeDelta, (pointer.ClampPointer) ? screenBoundScale : 1);
        pointerVisible = visibleInCanvas || pointer.ClampPointer;
        pointer.runtimeUI.SetActive(pointerVisible);
        if (pointerVisible)
        {
            if (pointer.ClampPointer)
            {
                pointer.runtimeUI.SetOffScreen(!visibleInCanvas, GetClampAngle(canvasPos));
                runtimeSize = pointer.runtimeUI.rectTransform.sizeDelta;
                minClamp = new Vector2(-canvasSize.x + runtimeSize.x, -canvasSize.y + runtimeSize.y);
                maxClamp = new Vector2(canvasSize.x - runtimeSize.x, canvasSize.y - runtimeSize.y);

                //Clamp
                canvasPos.x = Mathf.Clamp(canvasPos.x, minClamp.x, maxClamp.x);
                canvasPos.y = Mathf.Clamp(canvasPos.y, minClamp.y, maxClamp.y);
            }
            pointer.runtimeUI.SetAnchoredPosition(canvasPos);
            if (pointer.customData != null)
            {
                pointer.runtimeUI.UpdateInfo(pointer.customData);
            }
            if (pointer.CheckDistance)
            {
                targetDistance = Vector3.Distance(gameCamera.transform.position, pointer.target.position);
                pointer.runtimeUI.SetDistance(targetDistance);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void GetCanvasScale()
    {
        if (uiCanvas == null) return;
        //
        canvasSize = uiCanvas.GetCanvasHalfSize(screenBoundScale);
    }
    #endregion

    #region FUNCTIONS
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public BaseHudPointerData GetHudData(int ID)
    {
        if (!runtimePointers.ContainsKey(ID))
        {
            return null;
        }
        return runtimePointers[ID];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasPos"></param>
    /// <returns></returns>
    private float GetClampAngle(Vector3 canvasPos)
    {
        return Mathf.Atan2(canvasPos.y, canvasPos.x) * Mathf.Rad2Deg;
    }
    #endregion

    private static HudPointManager _instance = null;
    public static HudPointManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<HudPointManager>();
            }
            return _instance;
        }
    }
}
