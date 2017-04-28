using UnityEngine;
using System;
using Assets.Script.Base;
using Assets.Script.Tools;
using Assets.Script.Trash;

public class ControlManager : TSingleton<ControlManager>, IDisposable
{

    public bool CanControl;
    public bool IsTouch;
    #region private
    private Camera cam;
    private BaseTrash mSelectTrash;
    private int mTrashLayerIndex;
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
        mSelectTrash = null;
        RemoveListener();
    }

    public void InitComponent()
    {
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Input.multiTouchEnabled = false;
        }
    }

    public void InitData()
    {
        CanControl = true;
        IsTouch = false;
        mTrashLayerIndex = 8;
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
            mSelectTrash = HitTrash(PlatformTools.m_TouchPosition);
            if (mSelectTrash)
            {
                IsTouch = true;
                mSelectTrash.PickUpTrash();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (mSelectTrash)
            {
                IsTouch = false;
                mSelectTrash.ReleaseTrash();
                mSelectTrash = null;
               
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
        if (mSelectTrash != null)
        {
            newPosition = Camera.main.WorldToScreenPoint(mSelectTrash.CacheTrans.position);
            pos.z = newPosition.z;
            newPosition.x = cam.ScreenToWorldPoint(pos).x;// + offset.x;
            newPosition.y = cam.ScreenToWorldPoint(pos).y;// + offset.y;
            newPosition.z = mSelectTrash.CacheTrans.position.z;
            if (GameHelper.instance.InTheArea(newPosition) && GameHelper.instance.UnderGround(newPosition, 0) == false)
            {
                mSelectTrash.CacheTrans.position = newPosition;
            }
        }
    }

    private BaseTrash HitTrash(Vector3 pos)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(pos);

        if (Physics.Raycast(ray, out hit,100, 1 << mTrashLayerIndex))
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
