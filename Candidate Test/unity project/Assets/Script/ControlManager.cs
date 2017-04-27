using UnityEngine;
using System;
using Assets.Script.Base;
using Assets.Script.Tools;

public class ControlManager : TSingleton<ControlManager>, IDisposable
{
    #region private
    private Camera cam;
    #endregion
    public bool CanControl;
    public override void Init()
    {
        base.Init();
    }
    public override void Dispose()
    {
        base.Dispose();
        RemoveListener();
    }

    public void InitComponent()
    {
        cam = Camera.main;
    }

    public void InitData()
    {
        CanControl = true;
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

    public override void Update(float time)
    {
        base.Update(time);
        if (CanControl == false)
        {
            return;
        }

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                MouseMovePlayer(PlatformTools.m_TouchPosition);
            }
        }
        else
        {
            MouseMovePlayer(PlatformTools.m_TouchPosition);
        }
    }


    #region fuc

    /// <summary>
    /// 鼠标点击移动目标
    /// </summary>
    private void MouseMovePlayer(Vector3 pos)
    {
        Vector3 newPosition = Vector3.zero;
        newPosition = cam.ScreenToWorldPoint(pos);
        newPosition.z = 0;
    }
    #endregion
}
