using Assets.Script.Container;
using Assets.Script.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.GameState
{
    public class IntroSystem : BaseState
    {

        private enum IntroStateEnum
        {
            MoveContainer,
            Finish,
        }

        public override GameStateEnum CurrentState
        {
            get
            {
                return GameStateEnum.Intro;
            }
        }

        private IntroStateEnum mCurrentIntroState;

        public override void InitCompennet()
        {
        }

        public override void InitData()
        {
            ControlManager.instance.CanControl = false;
            mCurrentIntroState = IntroStateEnum.MoveContainer;
        }

        public override void Update()
        {
            switch (mCurrentIntroState)
            {
                case IntroStateEnum.MoveContainer:
                    CheckMoveFinish();
                    break;
                case IntroStateEnum.Finish:
                    Finish();
                    break;
            }
        }

        public override void Dispose()
        {
        }


        private void CheckMoveFinish()
        {
            bool bFinish = true;
            using (var container = ContainersDic.GetEnumerator())
            {
                while (container.MoveNext())
                {
                    bFinish &= container.Current.Value.IntroFinish;
                }
            }
            if (bFinish)
            {
                mCurrentIntroState = IntroStateEnum.Finish;
            }
        }

        private void Finish()
        {
            NextState = CurrentState + 1;
        }
    }
}
