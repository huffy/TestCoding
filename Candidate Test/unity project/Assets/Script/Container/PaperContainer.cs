using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

namespace Assets.Script.Container
{
    public class PaperContainer : BaseContainer
    {
        public override string ParentName
        {
            get
            {
                return "Paper";
            }
        }

        public override ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Paper;
            }
        }

    }
}
