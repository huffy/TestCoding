using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

namespace Assets.Script.Trash
{
    public class GlassTrash: BaseTrash
    {
        public override ContainerEnum TrashType
        {
            get
            {
                return ContainerEnum.Glass;
            }
        }

        public override float InitHeight
        {
            get
            {
                return 0.23f;
            }
        }

        public override float MaxHeightOffset
        {
            get
            {
                return 0.13f;
            }
        }

        public override float MinDistance
        {
            get
            {
                return 0.1f;
            }
        }

        public override SoundEnum TrashInCountainerSound
        {
            get
            {
                return SoundEnum.TrashintheBinGlass;
            }
        }

    }
}
