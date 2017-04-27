using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

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

        public override void InitCompennet()
        {
        }

        public override void InitData()
        {
            ControlManager.instance.CanControl = true;
        }

        public override void Update()
        {
        }
    }
}
