using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;
using Assets.Script.EventMgr;
using Assets.Script.Trash;

namespace Assets.Script.GameState
{
    public class PlayGameSystem : BaseState
    {
        public override GameStateEnum CurrentState
        {
            get
            {
                return GameStateEnum.PlayGame;
            }
        }

        public override void InitCompennet()
        {
            EventManager.instance.AddListener(EventDefineEnum.TrashInContainer, TrashInContainer);
        }

        public override void InitData()
        {
            ControlManager.instance.CanControl = true;
        }

        public override void Update()
        {
        }

        public override void Dispose()
        {
            EventManager.instance.RemoveListener(EventDefineEnum.TrashInContainer, TrashInContainer);
        }

        private void TrashInContainer(object obj, EventArgs e)
        {
            BaseTrash trash = (BaseTrash)obj;
            if (trash != null && TrashList.Contains(trash))
            {
                TrashList.Remove(trash);
            }
            if (TrashList.Count <= 0)
            {
                NextState = CurrentState + 1;
            }
        }
    }
}
