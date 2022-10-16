using System.Collections.Generic;
using UnityEngine;

public class HudPointManager : MonoBehaviour
{
    #region SERIALZIE FIELDS
    [Header("Settings")]
    [SerializeField] private bool useFrameDelay;
    [SerializeField] private int updateFrameDelay = 5;
    [Space]
    [Header("References")]
    [SerializeField] private GameObject defaultHudPrefab;
    [SerializeField] private RectTransform rootPanel;
    [SerializeField] private GameCanvas uiCanvas;
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
        if (Camera.main == null || runtimePointers.Count < 1)
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
    private float distance;
    private bool isVisible;
    private void UpdateHudPointer(BaseHudPointerData pointer, bool forceUpdate = false)
    {
        if (pointer.target == null || pointer.runtimeUI == null) return;

        //
        if (forceUpdate)
        {
            pointer.runtimeUI.SetAlpha(HudAlpha);
        }
        canvasPos = uiCanvas.WorldToCanvas(pointer.target.position + pointer.Offset, Camera.main);
        isVisible = uiCanvas.IsVisible(canvasPos, pointer.runtimeUI.rectTransform.sizeDelta) && !pointer.ClampPointer;
        pointer.runtimeUI.SetActive(isVisible);
        if (isVisible)
        {
            if (pointer.ClampPointer)
            {
                pointer.runtimeUI.SetOffScreen(GetClampDirection(canvasPos, pointer.runtimeUI));
            }
            pointer.runtimeUI.SetAnchoredPosition(canvasPos);
            if (pointer.customData != null)
            {
                pointer.runtimeUI.UpdateInfo(pointer.customData);
            }
            if (pointer.CheckDistance)
            {
                distance = Vector3.Distance(Camera.main.transform.position, pointer.target.position);
                pointer.runtimeUI.SetDistance(distance);
            }
        }
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
    /// <param name="hudPointUI"></param>
    /// <returns></returns>
    private int GetClampDirection(Vector3 canvasPos, HudPointUIBase hudPointUI)
    {
        Vector2 canvasSize = uiCanvas.GetCanvasHalfSize();
        if (canvasPos.x < -canvasSize.x + hudPointUI.rectTransform.sizeDelta.x)
        {
            return 180;
        }
        else if (canvasPos.x > canvasSize.x - hudPointUI.rectTransform.sizeDelta.x)
        {
            return 0;
        }
        else if (canvasPos.y < -canvasSize.y + hudPointUI.rectTransform.sizeDelta.y)
        {
            return 270;
        }
        else if (canvasPos.y > canvasSize.y - hudPointUI.rectTransform.sizeDelta.y)
        {
            return 90;
        }
        return -1;
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
