using UnityEngine;
using System.Collections;
using Assets.Script.Base;
using System;
using Assets.Script.Tools;
using System.Collections.Generic;
using Assets.Script.EventMgr;

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

        public bool RotateStart
        {
            get
            {
                return bCantRotate;
            }
        }

        public virtual ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Food;
            }
        }

        #endregion

        #region Component
        private Transform parentTrans;
        private Animation mAnim;
        private int realAngle;
        private Vector3 tempVector;
        private GameObject tempObj;
        #endregion

        private bool bCantRotate;
        private Queue<BinAnimationEnum> mAnimationQueue;

        public override void InitComponent()
        {
            base.InitComponent();
         
            mAnim = GetComponent<Animation>();
            if (mAnim == null)
            {
                mAnim = CacheObj.AddComponent<Animation>();
            }
            string parentPath = string.Format(StaticMemberMgr.SCENE_CONTAINERS_PATH+ "/{0}", ParentName);
            GameHelper.instance.GetTransformByPath(ref parentTrans, parentPath);
            mAnimationQueue = new Queue<BinAnimationEnum>();
        }

        public override void InitData()
        {
            base.InitData();
            bCantRotate = false;
            realAngle = (StaticMemberMgr.MAX_ANGLE + Angle) % StaticMemberMgr.MAX_ANGLE;
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
            if (parentTrans == null)
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

        public override void LogicCollision(BaseCreator creator)
        {
        }


        public void PlayAnimtion(BinAnimationEnum mAnimationType)
        {
            if (mAnim == null)
            {
                return;
            }

            if (mAnim.isPlaying)
            {
                mAnimationQueue.Enqueue(mAnimationType);
            }
            else
            {
                mAnim.Play(mAnimationType.ToString());
            }

        }

        public void PlaySound()
        {

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
                PlayAnimtion(BinAnimationEnum.BinClose);
            }
        }

        private void PickUpTrash(object obj, EventArgs e)
        {
            ContainerTypeParam type = (ContainerTypeParam)e;
            if (ContainerType == type.ContainerType)
            {
                PlayAnimtion(BinAnimationEnum.BinOpen);
            }
        }
        #endregion

        #region private 
        private void CheckRotate()
        {
            if (bCantRotate)
            {
                if (Mathf.Abs((int)CacheTrans.localEulerAngles.y - realAngle) > 1)
                {
                    tempVector = Vector3.down * Time.deltaTime * RotateSpeed;
                    CacheTrans.Rotate(tempVector);
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
        }
        #endregion
    }
}
