using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

namespace Assets.Script.Trash
{
    public class PlasticTrash : BaseTrash
    {
        public override ContainerEnum TrashType
        {
            get
            {
                return ContainerEnum.Plastic;
            }
        }

        public override float InitHeight
        {
            get
            {
                return 0.2f;
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
                return 0.12f;
            }
        }

        public override SoundEnum TrashInCountainerSound
        {
            get
            {
                return SoundEnum.TrashintheBinPlastics;
            }
        }
    }
}
