using Assets.Script.Container;
using Assets.Script.Tools;
using Assets.Script.Trash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.GameState
{
    public abstract class BaseState
    {
        public abstract GameStateEnum CurrentState
        {
            get;
        }
        public GameStateEnum NextState
        {
            get;
            protected set;
        }

        public Dictionary<int, BaseContainer> mContainersDic;
        public List<BaseTrash> mTrashList;
        public Transform mRootTrashTrans;

        public void Init(Dictionary<int, BaseContainer> containersDic, List<BaseTrash> trashList, Transform rootTrashTrans)
        {
            mContainersDic = containersDic;
            mTrashList = trashList;
            mRootTrashTrans = rootTrashTrans;
            InitCompennet();
            InitData();
        }
        public abstract void InitCompennet();
        public abstract void InitData();
        public abstract void Update();
    }
}
