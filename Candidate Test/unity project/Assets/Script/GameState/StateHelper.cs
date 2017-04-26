using Assets.Script.Base;
using Assets.Script.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.GameState
{
    public class StateHelper: TSingleton<StateHelper>
    {
        public BaseState SwitchState(GameStateEnum state)
        {
            switch (state)
            {
                case GameStateEnum.Intro:
                    return new IntroSystem();
                case GameStateEnum.PlayGame:
                    return new PlayGameSystem();
                case GameStateEnum.End:
                    return new PlayGameSystem();
            }

            DebugHelper.DebugLogError(" state is error ==" + state);
            return null;
        }
    }
}
