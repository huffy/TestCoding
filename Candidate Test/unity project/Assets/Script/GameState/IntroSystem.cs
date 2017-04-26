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
    
        public override GameStateEnum CurrentState
        {
            get
            {
                return GameStateEnum.Intro;
            }
        }

        public override void InitCompennet()
        {
            base.InitCompennet();
        }

        public override void InitData()
        {
            base.InitData();
        }


    }
}
