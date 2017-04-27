
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using Assets.Script.EventMgr;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Tools
{
    public class GameHelper : TSingleton<GameHelper>, IDisposable
    {

        public Camera MainCamera;
        public delegate void DoSomeHandle(EventNoticeParam param);
        public Vector3 LeftWallVec;
        public Vector3 RightWallVec;
        public Vector3 BottomWallVec;
        private GameObject tempObj;
        private Transform tempTrans;
        private List<Vector3> mPositionList;
        public override void Init()
        {
            base.Init();
            MainCamera = Camera.main;
            tempTrans = null;
            tempObj = null;
            if (MainCamera == null) { DebugHelper.DebugLogError(" MainCamera is Null"); }
            InitWallVector();
            mPositionList = new List<Vector3>();
        }

        public void GetTransformByPath(ref Transform trans, string path)
        {
             GetGameObjByPath(ref tempObj, path);
            if (tempObj != null)
            { trans = tempObj.transform; }
            else
            {
                DebugHelper.DebugLogError("TransformByPath error path = " + path);
            }
        }

        public void GetGameObjByPath(ref GameObject obj, string path)
        {
            obj = GameObject.Find(path);
            if (obj == null)
            { 
                DebugHelper.DebugLogError("GameObjByPath error path = " + path);
            }
        }

        /// <summary>
        /// 屏幕坐标和世界坐标转化
        /// </summary>
        public Vector3 GetScreenPos(Vector3 pos)
        {
            Vector3 screenPos = Vector3.zero;
            screenPos = MainCamera.WorldToScreenPoint(pos);
            return screenPos;
        }

        /// <summary>
        /// 判断是否在屏幕内
        /// </summary>
        public bool InTheArea(Vector3 worldPos)
        {
            return InTheArea(worldPos, RightWallVec, 0.5f) && InTheArea(worldPos, LeftWallVec,1.0f)
                || InTheArea(worldPos, RightWallVec, 1.0f) && InTheArea(worldPos, LeftWallVec, 0.5f);
        }

        /// <summary>
        /// 判断是否在屏幕内
        /// </summary>
        public bool InTheArea(Vector3 worldPos, Vector3 wallWolrdPos,float offset)
        {
            Vector2 screenPos = GetScreenPos(worldPos);
            Vector2 wallPos = GetScreenPos(wallWolrdPos);
            if (Mathf.Abs(screenPos.x - wallPos.x) < Screen.width * offset)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否到达地面
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="wallWolrdPos"></param>
        /// <param name="maxOffset">Random but != maxOffset</param>
        /// <returns></returns>
        public bool UnderGround(Vector3 worldPos, float maxOffset)
        {
            return UnderGround(worldPos, BottomWallVec, maxOffset);
        }

        /// <summary>
        /// 是否到达地面
        /// </summary>
        /// <param name="worldPos"></param>
        /// <param name="wallWolrdPos"></param>
        /// <param name="maxOffset">Random but != maxOffset</param>
        /// <returns></returns>
        public bool UnderGround(Vector3 worldPos, Vector3 wallWolrdPos, float maxOffset)
        {
            float offset = UnityEngine.Random.Range(0, maxOffset);
            if (worldPos.y - offset < wallWolrdPos.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsValidPos(Vector3 pos, float minDistance)
        {
            for (int i = 0; i < mPositionList.Count; i++)
            {
                if (Mathf.Abs(pos.x - mPositionList[i].x) < minDistance)
                {
                    return false;
                }
            }
            mPositionList.Add(pos);
            return true;
        }

        /// <summary>
        /// 计算两者距离
        /// </summary>
        /// <param name="target1Pos"></param>
        /// <param name="target2Pos"></param>
        /// <returns></returns>
        public float Distance(Vector3 target1Pos, Vector3 target2Pos)
        {
            float dis = 0;
            dis = Vector3.Distance(target1Pos, target2Pos);
            return dis;
        }

        public float Distance(Vector3 target1Pos, float target1Range, Vector3 target2Pos, float target2Range)
        {
            float dis = 0;
            dis = Distance(target1Pos, target2Pos);
            dis = dis - (target1Range + target2Range);

            return dis;
        }

        /// <summary>
        /// 设置物体的显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="bAtive"></param>
        /// <returns></returns>
        public bool SetGameObjActive<T>(T go, bool bAtive) where T : Component
        {
            if (go == null)
            {
                DebugHelper.DebugLogError("T  gameobject is null");
                return false;
            }
            else
            {
                go.gameObject.CustomSetActive(bAtive);
                return true;
            }
        }

        /// <summary>
        /// 获取物体显示的状态
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <returns></returns>
        public bool GetGameObjectActive<T>(T go, bool bActive) where T : Component
        {
            if (go == null)
            {
                DebugHelper.DebugLogError("GetGameObjectActive T is null");
                return false;
            }
            else
            {
                return go.gameObject.activeSelf == bActive;
            }
        }


        public string GetPrefabPath(string name, string path = "")
        {
            //string mPath = "";
            if (string.IsNullOrEmpty(path))
                return  string.Format("Prefab/{0}", name);
            else
                return string.Format("Prefab/{0}/{1}", path, name);
        }

        /// <summary>
        /// 延迟做
        /// </summary>
        /// <param name="DelayTime"> == 0 则延迟一帧 </param>
        /// <param name="mDoSomeHandle"></param>
        /// <returns></returns>
        public IEnumerator DelayHandle(float DelayTime, DoSomeHandle mDoSomeHandle, EventNoticeParam param = null)
        {
            if (DelayTime <= 0)
            {
                yield return new  WaitForEndOfFrame();
            }
            else
            {
                yield return new WaitForSeconds(DelayTime);
            }
            mDoSomeHandle(param);
        }

        public override void Dispose()
        {
            tempTrans = null;
            tempObj = null;
            base.Dispose();
        }

        private void InitWallVector()
        {
            GetTransformByPath(ref tempTrans, StaticMemberMgr.WALL_LEFT_PATH);
            if (tempTrans != null)
            {
                LeftWallVec = tempTrans.position;
            }
            GetTransformByPath(ref tempTrans, StaticMemberMgr.WALL_RIGHT_PATH);
            if (tempTrans != null)
            {
                RightWallVec = tempTrans.position;
            }
            GetTransformByPath(ref tempTrans, StaticMemberMgr.WALL_BOTTOM_PATH);
            if (tempTrans != null)
            {
                BottomWallVec = tempTrans.position;
            }
        }
    }
}
