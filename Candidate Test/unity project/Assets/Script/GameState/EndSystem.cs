using Assets.Script.Tools;
using UnityEngine;
using Assets.Script.AudioMgr;

namespace Assets.Script.GameState
{
    public class EndSystem : BaseState
    {
        public override GameStateEnum CurrentState
        {
            get
            {
                return GameStateEnum.End;
            }
        }
        private Transform mParticleTrans;
        private string mPartaicleName;
        public override void InitCompennet()
        {
            mPartaicleName = "StarParticle";
        }

        public override void InitData()
        {
            ControlManager.instance.CanControl = true;
            Transform trans = null;
            GameHelper.instance.GetTransformByPath(ref trans, StaticMemberMgr.SCENE_OBJ_NAME);
            if (trans != null)
            {
                mParticleTrans = trans.FindChild(mPartaicleName);
                mParticleTrans.CustomSetActive(true);
                AudioControl.instance.PlayAudio((int)SoundEnum.EndMagicParticle);
            }
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            mParticleTrans.CustomSetActive(false);
        }
    }
}
