using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Tools;

namespace Assets.Script.Container
{
    public class PlasticContainer : BaseContainer
    {
        public override string ParentName
        {
            get
            {
                return "Plastic";
            }
        }

        public override ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Plastic;
            }
        }
    }
}
