
//功能： 游戏总控制
//创建者: 胡海辉
//创建时间：


using Assets.Script.AudioMgr;
using Assets.Script.Base;
using Assets.Script.Container;
using Assets.Script.GameState;
using Assets.Script.Tools;
using Assets.Script.Trash;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Script
{
    public class GameLogic : BaseMonoBehaviour
    {
        public float audioTime = 0.5f;
        private BaseState mGamestate;
        private Dictionary<int, BaseContainer> mContainersDic;
        private List<BaseTrash> mTrashList;
        private Transform mRootContainerTrans;
        private Transform mRootTrashTrans;
        public override void Awake()
        {
            DebugHelper.bEnableDebug = true;
            base.Awake();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public override void Init()
        {
            AudioControl.CreateInstance();
            GameHelper.CreateInstance();
            ControlManager.CreateInstance();
            base.Init();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public override void InitComponent()
        {
            base.InitComponent();
            mContainersDic = new Dictionary<int, BaseContainer>((int)ContainerEnum.Max);
            mTrashList = new List<BaseTrash>(StaticMemberMgr.MAX_TRASH);
            string path = StaticMemberMgr.SCENE_CONTAINERS_PATH;
            GameHelper.instance.GetTransformByPath(ref mRootContainerTrans, path);
            path = StaticMemberMgr.SCENE_TRASH_PATH;
            GameHelper.instance.GetTransformByPath(ref mRootTrashTrans, path);
            InitContainer();
            InitTrash();
        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public override void InitListener()
        {
            base.InitListener();
        }

        public override void InitData()
        {
            base.InitData();
            mGamestate = StateHelper.instance.SwitchState(GameStateEnum.Intro);
            mGamestate.Init(mContainersDic, mTrashList, mRootTrashTrans);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public override void RemoveListener()
        {
            base.RemoveListener();
        }


        public override void Update()
        {
            base.Update();
            if (AudioControl.HasInstance())
            {
                AudioControl.instance.Update(audioTime);
            }
            if (ControlManager.HasInstance())
            {
                ControlManager.instance.Update(Time.deltaTime);
            }
        }

        public override void OnDestroy()
        {
            ControlManager.DestroyInstance();
            GameHelper.DestroyInstance();
            ControlManager.DestroyInstance();
            base.OnDestroy();
        }

        #region container
        private void InitContainer()
        {
            if (mRootContainerTrans == null)
            {
                DebugHelper.DebugLogError("InitContainer ---  mRootContainerTrans==  null");
                return;
            }

            for (int i = 0; i < (int)ContainerEnum.Max; i++)
            {
                GetContainerByType(mRootContainerTrans, (ContainerEnum)i);
            }
        }

        private void GetContainerByType(Transform root, ContainerEnum containerType)
        {
            if (mContainersDic == null)
            {
                DebugHelper.DebugLogError("GetContainerByType----  mContainersDic not init ");
                return;
            }

            string path = string.Format("{0}/{0}Container", containerType);
            int containersdicKey = (int)containerType;
            BaseContainer container = root.FindChildComponent<BaseContainer>(path);

            if (mContainersDic.ContainsKey(containersdicKey) == false)
            {
                mContainersDic.Add(containersdicKey, container);
            }
            else
            {
                mContainersDic[containersdicKey] = container;
            }
        }
        #endregion
        #region Trash
        private void InitTrash()
        {
            if (mRootTrashTrans == null)
            {
                DebugHelper.DebugLogError("InitTrash ---  mRootTrashTrans==  null");
                return;
            }
            GetTrashByType(mRootTrashTrans);
        }

        private void GetTrashByType(Transform root)
        {
            if (mTrashList == null)
            {
                DebugHelper.DebugLogError("GetTrashByType----  mTrashList not init ");
                return;
            }
            BaseTrash trash;
            for (int i = 0; i < root.childCount; i++)
            {
                trash = root.GetChild(i).GetComponent<BaseTrash>();
                if (trash != null)
                {
                    mTrashList.Add(trash);
                }
                else
                {
                    DebugHelper.DebugLogErrorFormat("GetTrashByType---- {0} not BaseTrash ", root.GetChild(i).name);
                }
            }
        }
        #endregion
    }
}
