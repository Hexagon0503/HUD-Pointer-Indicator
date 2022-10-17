using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudPoint : MonoBehaviour
{
    #region SERIALIZE FIELD
    /// <summary>
    /// 
    /// </summary>
    public HudPointData info;
    #endregion

    #region FIELDS
    /// <summary>
    /// 
    /// </summary>
    private int ID = -1;

    /// <summary>
    /// 
    /// </summary>
    private HudPointManager hudManager;
    #endregion

    #region UNITY METHODS
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        hudManager = HudPointManager.Instance;
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable()
    {
        RegisterPoint();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable()
    {
        RemovePoint();
    }
    #endregion

    #region METHODS
    /// <summary>
    /// 
    /// </summary>
    public void RegisterPoint()
    {
        if (!hudManager) return;
        //
        ID = hudManager.RegisterPoint(info);
    }

    /// <summary>
    /// 
    /// </summary>
    public void RemovePoint()
    {
        if (ID != -1)
        {
            if (hudManager)
            {
                hudManager.RemovePoint(ID);
            }
            ID = -1;
        }
    }
    #endregion
}
