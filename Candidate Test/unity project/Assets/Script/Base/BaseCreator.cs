
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using Assets.Script.Tools;
using UnityEngine;

namespace Assets.Script.Base
{
    public abstract class BaseCreator : BaseMonoBehaviour
    {
        [HideInInspector]
        public BaseCreator mCreator;

        public virtual ActorTypeEnum mActorType
        {
            get
            {
                return ActorTypeEnum.Cotainer;
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Dispose();
        }

        public virtual void SetBaseCreator(BaseCreator creator) 
        {
            mCreator = creator;
        }
        public abstract void LogicCollision(BaseCreator creator, ColliderStateEnum colliderState);

        public abstract void PlayGameSound(SoundEnum soundType);

        public abstract void Dispose();
    }
}
