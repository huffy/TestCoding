using UnityEngine;
using System.Collections;
using Assets.Script.Base;
using Assets.Script.Tools;
using Assets.Script.EventMgr;

namespace Assets.Script.Trash
{
    public class BaseTrash : BaseCreator
    {
        #region public
        public bool ArriveGround
        {
            get
            {
                return bArriveGround;
            }
        }
        public virtual ContainerEnum TrashType
        {
            get
            {
                return ContainerEnum.Food;
            }
        }
        public virtual float InitHeight
        {
            get
            {
                return 0.5f;
            }
        }

        public virtual float MaxHeightOffset
        {
            get
            {
                return 0.1f;
            }
        }

        public virtual float MinDistance
        {
            get
            {
                return 0.1f;
            }
        }

        #endregion

        private bool bArriveGround;
        private Rigidbody mRigidbody;
        private float mLeftMax, mRightMax;

        public override void InitComponent()
        {
            base.InitComponent();
            mRigidbody = GetComponent<Rigidbody>();
            DebugHelper.AssertError(mRigidbody == null, "Rigidbody component is null ");
        }

        public override void InitData()
        {
            base.InitData();
            InitTrashPos();
            mRigidbody.useGravity = true;
        }

        public override void Update()
        {
            base.Update();
            if (bArriveGround == false)
            {
                bArriveGround = CheckArriveGround();
            }
        }

        public override void SetBaseCreator(BaseCreator creator)
        {
            base.SetBaseCreator(creator);
        }

        public override void LogicCollision(BaseCreator creator)
        {
        }

        public void ReleaseTrash()
        {
            if (mRigidbody)
            {
                mRigidbody.useGravity = true;
            }
            bArriveGround = false;
            ContainerTypeParam trashType = new ContainerTypeParam(TrashType);
            EventManager.instance.RasieEvent(EventDefineEnum.ReleaseTrash, CacheObj, trashType);
        }

        public void PickUpTrash()
        {
            ContainerTypeParam trashType = new ContainerTypeParam(TrashType);
            EventManager.instance.RasieEvent(EventDefineEnum.PickUpTrash, CacheObj, trashType);
        }

        public void StopRigidbody()
        {
            if (mRigidbody)
            {
                mRigidbody.useGravity = false;
                mRigidbody.Sleep();
            }
        }

        private void InitTrashPos()
        {
            int loopCount = 0, maxLoopCount = 20;
            bArriveGround = false;
            mLeftMax = GameHelper.instance.LeftWallVec.x;
            mRightMax = GameHelper.instance.RightWallVec.x;
            Vector3 initPos = new Vector3(Random.Range(mLeftMax, mRightMax), InitHeight, 0);
            while (GameHelper.instance.IsValidPos(initPos, MinDistance) == false)
            {
                loopCount++;
                if (loopCount > maxLoopCount)
                {
                    DebugHelper.DebugLogError(" position is error!!!  name==" + CacheTrans.name);
                    break;
                }
                initPos = new Vector3(Random.Range(mLeftMax, mRightMax), InitHeight, 0);
            }
            CacheTrans.localPosition = initPos;
        }

        private bool CheckArriveGround()
        {
            bool bArrive;
            bArrive = GameHelper.instance.UnderGround(CacheTrans.localPosition, MaxHeightOffset);
            if (bArrive)
            {
                StopRigidbody();
            }
            return bArrive;
        }

      
    }
}
