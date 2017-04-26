using UnityEngine;
using System.Collections;
using Assets.Script.Base;
using System;
using Assets.Script.Tools;

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

        public GameObject cacheObj
        {
            get;
            private set;
        }
        public Transform cacheTrans
        {
            get;
            private set;
        }

        #endregion

        #region Component
        private Transform parentTrans;
        private Animation anim;
        private int realAngle;
        private Vector3 tempVector;
        private GameObject tempObj;
        #endregion

        private bool bCantRotate;

        public override void InitComponent()
        {
            base.InitComponent();
            cacheObj = gameObject;
            cacheTrans = transform;
            anim = GetComponent<Animation>();
            if (anim == null)
            {
                anim = cacheObj.AddComponent<Animation>();
            }
            string parentPath = string.Format(StaticMemberMgr.SCENE_CONTAINERS_PATH+ "/{0}", ParentName);
            GameHelper.instance.GetTransformByPath(ref parentTrans, parentPath);
        }

        public override void InitData()
        {
            base.InitData();
            bCantRotate = false;
            realAngle = (StaticMemberMgr.MAX_ANGLE + Angle) % StaticMemberMgr.MAX_ANGLE;
            PlayAnimtion(BinAnimationEnum.BinRollIn);
        }

        public void PlayAnimtion(BinAnimationEnum mAnimationType)
        {
            anim.Play(mAnimationType.ToString());
        }

        public void PlaySound()
        {

        }

        public void StartRotate()
        {
            bCantRotate = true;
        }

        public override void Update()
        {
            base.Update();
            if (parentTrans == null)
            {
                return;
            }
            if (bCantRotate)
            {
                if ((int)cacheTrans.localEulerAngles.y != realAngle)
                {
                    tempVector = Vector3.up * Time.deltaTime * RotateSpeed;
                    if (Angle < 0)
                    {
                        tempVector *= -1;
                    }

                    cacheTrans.Rotate(tempVector);
                }
                else
                {
                    bCantRotate = false;

                }
            }
        }

        public override void SetBaseCreator(BaseCreator creator)
        {
            base.SetBaseCreator(creator);
        }

        public override void LogicCollision(BaseCreator creator)
        {
        }


    }
}
