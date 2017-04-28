using UnityEngine;
using System;
using Assets.Script.Base;
using Assets.Script.Tools;
using Assets.Script.Trash;

public class ControlManager : TSingleton<ControlManager>, IDisposable
{

    public bool CanControl;

    #region private
    private Camera cam;
    private BaseTrash selectTrash;
    private int trashLayerIndex;
    #endregion

    public override void Init()
    {
        base.Init();
        InitComponent();
        InitData();
        InitListener();
    }
    public override void Dispose()
    {
        base.Dispose();
        RemoveListener();
    }

    public void InitComponent()
    {
    }

    public void InitData()
    {
        CanControl = true;
        trashLayerIndex = 8;
    }

    /// <summary>
    /// 初始化监听
    /// </summary>
    public void InitListener()
    {
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    public void RemoveListener()
    {
    }

    public void InitCamera(Camera mainCamera)
    {
        cam = mainCamera;
    }

    public override void Update(float time)
    {
        base.Update(time);
        if (CanControl == false)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            selectTrash = HitTrash(PlatformTools.m_TouchPosition);
            if (selectTrash)
            {
                selectTrash.PickUpTrash();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (selectTrash)
            {
                selectTrash.ReleaseTrash();
                selectTrash = null;
            }
        }

        if (Input.GetMouseButton(0))
        {
            MouseMove(PlatformTools.m_TouchPosition);
        }
    }

    #region fuc

    /// <summary>
    /// 鼠标点击移动目标
    /// </summary>
    private void MouseMove(Vector3 pos)
    {
        Vector3 newPosition = Vector3.zero;
        if (selectTrash != null)
        {
            newPosition = Camera.main.WorldToScreenPoint(selectTrash.CacheTrans.position);
            pos.z = newPosition.z;
            newPosition.x = cam.ScreenToWorldPoint(pos).x;// + offset.x;
            newPosition.y = cam.ScreenToWorldPoint(pos).y;// + offset.y;
            newPosition.z = selectTrash.CacheTrans.position.z;
            if (GameHelper.instance.InTheArea(newPosition) && GameHelper.instance.UnderGround(newPosition, 0) == false)
            {
                selectTrash.CacheTrans.position = newPosition;
            }
        }
    }

    private BaseTrash HitTrash(Vector3 pos)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit,100, 1 << trashLayerIndex))
        {
            return hit.transform.GetComponent<BaseTrash>();
        }
        else
        {
            return null;
        }
    }
    #endregion
}
