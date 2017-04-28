using UnityEngine;
using System.Collections;
using Assets.Script.Base;
using Assets.Script.Tools;
using Assets.Script.EventMgr;
using Assets.Script.AudioMgr;

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

        public virtual SoundEnum TrashInCountainerSound
        {
            get
            {
                return SoundEnum.PickUpTrash;
            }
        }

        public override ActorTypeEnum mActorType
        {
            get
            {
                return ActorTypeEnum.Trash;
            }
        }
        public bool IsPickUp;
        #endregion

        private const float NEED_MOVE_TIME = 0.2f;
        private const float DELAY_RASIE_EVENT_TIME = 0.1f;
        private const float MOVE_SPEED = 0.005f;

        private bool bArriveGround;
        private bool bRightRelease;
        private bool bNeedMove;
        private int mMoveDir;
        private float mAddTime;
        private Rigidbody mRigidbody;
        private float mLeftMax, mRightMax;
        private Vector3 newPos;

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
            mAddTime = 0;
            IsPickUp = false;
            mRigidbody.useGravity = true;
        }

        public override void Update()
        {
            base.Update();
            if (bArriveGround == false)
            {
                bArriveGround = CheckArriveGround();
            }
            if (bNeedMove)
            {
                bNeedMove = Move();
            }
        }

        public override void SetBaseCreator(BaseCreator creator)
        {
            base.SetBaseCreator(creator);
        }

        /// <summary>
        /// collision
        /// </summary>
        /// <param name="creator">主动来撞的</param>
        /// <param name="colliderState"></param>
        public override void LogicCollision(BaseCreator creator, ColliderStateEnum colliderState)
        {
            if (creator.mActorType == mActorType)
            {
                mAddTime = 0;
                bNeedMove = true;
                newPos = CacheTrans.localPosition;
                if (creator.CacheTrans.position.x > CacheTrans.position.x)
                {
                    mMoveDir = -1;
                }
                else
                {
                    mMoveDir = 1;
                }
            }
        }

        public override void PlayGameSound(SoundEnum soundType)
        {
            AudioControl.instance.PlayAudio((int)soundType);
        }

        public override void Dispose()
        {
            mRigidbody = null;
            bArriveGround = false;
            mCreator = null;
        }

        public void ReleaseTrash()
        {
            if (mRigidbody)
            {
                mRigidbody.useGravity = true;
            }
            bArriveGround = false;
            IsPickUp = false;
            StartCoroutine("DelayRasieEvent");
        }

        public void PickUpTrash()
        {
            ContainerTypeParam trashType = new ContainerTypeParam(TrashType);
            EventManager.instance.RasieEvent(EventDefineEnum.PickUpTrash, CacheObj, trashType);
            IsPickUp = true;
            PlayGameSound(SoundEnum.PickUpTrash);
        }

        public void StopRigidbody()
        {
            if (mRigidbody)
            {
                mRigidbody.useGravity = false;
                mRigidbody.Sleep();
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            OnTriggerState(other.transform, ColliderStateEnum.Enter);
        }

        public void OnTriggerStay(Collider other)
        {
            OnTriggerState(other.transform, ColliderStateEnum.Stay);
        }

        public void OnTriggerExit(Collider other)
        {
            OnTriggerState(other.transform, ColliderStateEnum.Exit);
        }

        private void InitTrashPos()
        {
            int loopCount = 0, maxLoopCount = 20;
            bArriveGround = false;
            mLeftMax = GameHelper.instance.LeftWallVec.x;
            mRightMax = GameHelper.instance.RightWallVec.x;
            newPos = new Vector3(Random.Range(mLeftMax, mRightMax), InitHeight, 0);
            while (GameHelper.instance.IsValidPos(newPos, MinDistance) == false)
            {
                loopCount++;
                if (loopCount > maxLoopCount)
                {
                    DebugHelper.DebugLogError(" position is error!!!  name==" + CacheTrans.name);
                    break;
                }
                newPos = new Vector3(Random.Range(mLeftMax, mRightMax), InitHeight, 0);
            }
            CacheTrans.localPosition = newPos;
        }

        private void OnTriggerState(Transform trans, ColliderStateEnum colliderstate)
        {
            ContainerKind container = trans.GetComponent<ContainerKind>();
            if (container != null)
            {
                if (TrashType == container.ContainerType && ControlManager.instance.IsTouch == false && bRightRelease == false)
                {
                    bRightRelease = true;
                    CacheTrans.position = new Vector3(trans.position.x, CacheTrans.position.y, trans.position.z);
                    PlayGameSound(TrashInCountainerSound);
                    return;
                }
            }

            BaseCreator creator = trans.GetComponent<BaseCreator>();
            if (creator == null)
            {
                return;
            }

            if (creator.mActorType != mActorType)
            {
                creator.SetBaseCreator(this);
                creator.LogicCollision(this, colliderstate);
                StopRigidbody();
            }
            else
            {
                if (IsPickUp || bArriveGround)
                {
                    creator.SetBaseCreator(this);
                    creator.LogicCollision(this, colliderstate);
                }
            }
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

        private IEnumerator DelayRasieEvent()
        {
            yield return new WaitForSeconds(DELAY_RASIE_EVENT_TIME);
            if (bRightRelease == false)
            {
                ContainerTypeParam trashType = new ContainerTypeParam(TrashType);
                EventManager.instance.RasieEvent(EventDefineEnum.ReleaseTrash, CacheObj, trashType);
            }
        }

        private bool Move()
        {
            mAddTime += Time.deltaTime;
            if (mAddTime < NEED_MOVE_TIME)
            {
                newPos += Vector3.right * MOVE_SPEED * mMoveDir;
                if (GameHelper.instance.InTheArea(newPos))
                {
                    CacheTrans.localPosition = newPos;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
