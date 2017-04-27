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

        public Dictionary<int, BaseContainer> ContainersDic;
        public List<BaseTrash> TrashList;
        public Transform RootTrashTrans;

        public void Init(Dictionary<int, BaseContainer> containersDic, List<BaseTrash> trashList, Transform rootTrashTrans)
        {
            ContainersDic = containersDic;
            TrashList = trashList;
            RootTrashTrans = rootTrashTrans;
            InitCompennet();
            InitData();
        }
        public abstract void InitCompennet();
        public abstract void InitData();
        public abstract void Update();
    }
}
