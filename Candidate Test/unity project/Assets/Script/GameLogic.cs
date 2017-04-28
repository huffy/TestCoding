
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
using Assets.Script.EventMgr;

namespace Assets.Script
{
    public class GameLogic : BaseMonoBehaviour
    {
        public float audioTime = 0.5f;
        public Camera MainCamera;
        private BaseState mGamestate;
        private GameStateEnum mCurrentState;
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
            EventManager.CreateInstance();
            base.Init();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public override void InitComponent()
        {
            base.InitComponent();
            ControlManager.instance.InitCamera(MainCamera);
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

        /// <summary>
        /// 初始化数据
        /// </summary>
        public override void InitData()
        {
            base.InitData();
            mCurrentState = GameStateEnum.Intro;
            mGamestate = SwitchState(mCurrentState);
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
            if (mGamestate != null)
            {
                if (mCurrentState < mGamestate.NextState)
                {
                    mCurrentState = mGamestate.NextState;
                    mGamestate = SwitchState(mCurrentState);
                    mGamestate.Init(mContainersDic, mTrashList, mRootTrashTrans);
                }
                mGamestate.Update();
            }

        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            ControlManager.DestroyInstance();
            GameHelper.DestroyInstance();
            ControlManager.DestroyInstance();
            EventManager.DestroyInstance();
            Dispose();
        }

        private void Dispose()
        {
            mContainersDic.Clear();
            mContainersDic = null;
            mTrashList.Clear();
            mTrashList = null;
            mGamestate = null;
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

        /// <summary>
        /// switch game step 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public BaseState SwitchState(GameStateEnum state)
        {
            switch (state)
            {
                case GameStateEnum.Intro:
                    return new IntroSystem();
                case GameStateEnum.PlayGame:
                    return new PlayGameSystem();
                case GameStateEnum.End:
                    return new PlayGameSystem();
            }
            DebugHelper.DebugLogError(" state is error ==" + state);
            return null;
        }
    }
}
