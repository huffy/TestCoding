using UnityEngine;
using System.Collections;
using Assets.Script.Base;

namespace Assets.Script.Trash
{
    public class BaseTrash : BaseCreator
    {
        public override void SetBaseCreator(BaseCreator creator)
        {
            base.SetBaseCreator(creator);
        }

        public override void LogicCollision(BaseCreator creator)
        {
        }
    }
}
