using UnityEngine;
using System;
using Assets.Script.Base;
using Assets.Script.Tools;
using Assets.Script.Trash;

public class ControlManager : TSingleton<ControlManager>, IDisposable
{
    #region private
    private Camera cam, UICam;
    #endregion
    private BaseTrash selectTrash;
    private int trashLayerIndex;
    public bool CanControl;
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

    public void InitCamera(Camera mainCamera, Camera UICamera)
    {
        cam = mainCamera;
        UICam = UICamera;
    }

    public override void Update(float time)
    {
        base.Update(time);
        if (CanControl == false)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            MouseMovePlayer(PlatformTools.m_TouchPosition);
            Debug.Log("==ScreenSpace===" + UICam.ScreenToWorldPoint(PlatformTools.m_TouchPosition));
        }
        else
        {

        }
    }


    #region fuc

    /// <summary>
    /// 鼠标点击移动目标
    /// </summary>
    private void MouseMovePlayer(Vector3 pos)
    {
        Vector3 newPosition = Vector3.zero;
        selectTrash = HitTrash(pos);
        if (selectTrash != null)
        {
            //Vector3 ScreenSpace = cam.WorldToScreenPoint(selectTrash.CacheTrans.position);
            //Vector3 offset = selectTrash.CacheTrans.position - UICam.ScreenToWorldPoint(new Vector3(pos.x, pos.y, ScreenSpace.z));
            //newPosition = new Vector3(pos.x, pos.y, ScreenSpace.z);
            //Debug.Log("== " + newPosition + " ScreenSpace===" + ScreenSpace + " pos===  " + pos);
            newPosition = UICam.ScreenToViewportPoint(pos); //+ offset;
            newPosition.z = selectTrash.CacheTrans.position.z;
            //Debug.Log(" newPosition === "+ newPosition + " ScreenSpace===" + ScreenSpace + " pos===  "+ pos);
            selectTrash.CacheTrans.position = newPosition;
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
