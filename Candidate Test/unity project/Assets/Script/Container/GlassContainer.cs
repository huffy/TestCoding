using Assets.Script.Tools;

namespace Assets.Script.Container
{
    public class GlassContainer : BaseContainer
    {
        public override string ParentName
        {
            get
            {
                return "Glass";
            }
        }

        public override ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Glass;
            }
        }
    }
}
