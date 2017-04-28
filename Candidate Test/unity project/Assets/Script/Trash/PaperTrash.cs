﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

namespace Assets.Script.Trash
{
    public class PaperTrash: BaseTrash
    {
        public override ContainerEnum TrashType
        {
            get
            {
                return ContainerEnum.Paper;
            }
        }

        public override float InitHeight
        {
            get
            {
                return 0.55f;
            }
        }

        public override float MaxHeightOffset
        {
            get
            {
                return 0.12f;
            }
        }

        public override float MinDistance
        {
            get
            {
                return 0.11f;
            }
        }
    }
}
