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
    public class BaseState
    {
        public virtual GameStateEnum CurrentState
        {
            get
            {
                return GameStateEnum.None;
            }
        }
        public Dictionary<int, BaseContainer> mContainers;
        public List<BaseTrash> mTrashList;
        public Transform mRootTrashTrans;

        public void Init(Dictionary<int, BaseContainer> containers, List<BaseTrash> trashList, Transform rootTrashTrans)
        {
            mContainers = containers;
            mTrashList = trashList;
            mRootTrashTrans = rootTrashTrans;
            InitCompennet();
            InitData();
        }
        public virtual void InitCompennet()
        {

        }

        public virtual void InitData()
        {

        }

        public BaseState ChangeNextState()
        {
            return StateHelper.instance.SwitchState(CurrentState + 1);
        }
    }
}
