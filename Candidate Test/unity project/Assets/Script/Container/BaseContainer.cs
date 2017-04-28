using UnityEngine;
using System.Collections;
using Assets.Script.Base;
using System;
using Assets.Script.Tools;
using System.Collections.Generic;
using Assets.Script.EventMgr;
using Assets.Script.AudioMgr;

namespace Assets.Script.Container
{
    public class BaseContainer : BaseCreator
    {
        #region Show Inspector
        public int showAngle = 0;
        public int rotateSpeed = 0;
        #endregion

        #region pubilc 

        public virtual String ParentName
        {
            get
            {
                return "";
            }
        }

        public int Angle
        {
            get
            {
                return showAngle;
            }
        }

        public int RotateSpeed
        {
            get
            {
                return rotateSpeed;
            }
        }

        public bool IntroFinish
        {
            get
            {
                return bIntroFinish;
            }
        }

        public virtual ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Food;
            }
        }
      
        public override ActorTypeEnum mActorType
        {
            get
            {
                return ActorTypeEnum.Cotainer;
            }
        }


        #endregion

        #region Component
        private Transform mParentTrans;
        private Animation mAnim;
        #endregion

        private int mRealAngle;
        private Vector3 mTempVector;
        private bool bCantRotate;
        private bool bIntroFinish;
        private Queue<BinAnimationEnum> mAnimationQueue;
        private bool firstSound;

        public override void InitComponent()
        {
            base.InitComponent();

            mAnim = GetComponent<Animation>();
            if (mAnim == null)
            {
                mAnim = CacheObj.AddComponent<Animation>();
            }
            string parentPath = string.Format(StaticMemberMgr.SCENE_CONTAINERS_PATH + "/{0}", ParentName);
            GameHelper.instance.GetTransformByPath(ref mParentTrans, parentPath);
            mAnimationQueue = new Queue<BinAnimationEnum>();
        }

        public override void InitData()
        {
            base.InitData();
            bCantRotate = false;
            firstSound = true;
            mRealAngle = (StaticMemberMgr.MAX_ANGLE + Angle) % StaticMemberMgr.MAX_ANGLE;
            PlayAnimtion(BinAnimationEnum.BinClose);
            PlayAnimtion(BinAnimationEnum.BinRollIn);
            PlayAnimtion(BinAnimationEnum.BinOpen);
            PlayAnimtion(BinAnimationEnum.BinClose);
        }

        public override void InitListener()
        {
            base.InitListener();
            EventManager.instance.AddListener(EventDefineEnum.ReleaseTrash, ReleaseTrash);
            EventManager.instance.AddListener(EventDefineEnum.PickUpTrash, PickUpTrash);
        }

        public override void RemoveListener()
        {
            base.RemoveListener();
            EventManager.instance.RemoveListener(EventDefineEnum.ReleaseTrash, ReleaseTrash);
            EventManager.instance.RemoveListener(EventDefineEnum.PickUpTrash, PickUpTrash);
        }

        public override void Update()
        {
            base.Update();
            if (mParentTrans == null)
            {
                return;
            }
            CheckRotate();
            PlayQueueAnimation();
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
            if (creator.mActorType == ActorTypeEnum.Trash)
            {
                creator.Dispose();
                creator.gameObject.CustomSetActive(false);
                EventManager.instance.RasieEvent(EventDefineEnum.TrashInContainer, creator, null);
                PlayAnimtion(BinAnimationEnum.BinClose, true);
            }
        }

        public override void Dispose()
        {
            mCreator = null;
            mParentTrans = null;
            mAnim = null;
            RemoveListener();
            mAnimationQueue.Clear();
            mAnimationQueue = null;
        }

        public void PlayAnimtion(BinAnimationEnum animationType, bool forcePlay = false)
        {
            if (mAnim == null)
            {
                return;
            }

            if (mAnim.isPlaying)
            {
                if (forcePlay)
                {
                    mAnim.Stop();
                    mAnimationQueue.Clear();
                    mAnim.Play(animationType.ToString());
                }
                else
                {
                    mAnimationQueue.Enqueue(animationType);
                }
            }
            else
            {
                mAnim.Play(animationType.ToString());
            }

        }
         
        public override void PlayGameSound(SoundEnum soundType)
        {
            if (firstSound && soundType == SoundEnum.CloseBin)
            {
                firstSound = false;
                return;
            }
            AudioControl.instance.PlayAudio((int)soundType);
        }

        public void StartRotate()
        {
            bCantRotate = true;
        }

        #region event
        private void ReleaseTrash(object obj, EventArgs e)
        {
            ContainerTypeParam type = (ContainerTypeParam)e;
            if (ContainerType == type.ContainerType)
            {
                PlayAnimtion(BinAnimationEnum.BinClose, true);
            }
        }

        private void PickUpTrash(object obj, EventArgs e)
        {
            ContainerTypeParam type = (ContainerTypeParam)e;
            if (ContainerType == type.ContainerType)
            {
                PlayAnimtion(BinAnimationEnum.BinOpen, true);
            }
        }
        #endregion

        #region private 
        private void CheckRotate()
        {
            if (bCantRotate)
            {
                if (Mathf.Abs((int)CacheTrans.localEulerAngles.y - mRealAngle) > 1)
                {
                    mTempVector = Vector3.down * Time.deltaTime * RotateSpeed;
                    CacheTrans.Rotate(mTempVector);
                }
                else
                {
                    bCantRotate = false;
                }
            }
        }

        private void PlayQueueAnimation()
        {
            if (mAnimationQueue.Count > 0)
            {
                if (mAnim.isPlaying == false)
                {
                    BinAnimationEnum animType = mAnimationQueue.Dequeue();
                    mAnim.Play(animType.ToString());
                }
            }
            else
            {
                if (bIntroFinish == false)
                {
                    bIntroFinish = !bIntroFinish;
                }
            }
        }
        #endregion

    }
}
