using Assets.Script.Tools;

namespace Assets.Script.Container
{
    public class FoodContainer : BaseContainer
    {
        public override string ParentName
        {
            get
            {
                return "Food";
            }
        }

        public override ContainerEnum ContainerType
        {
            get
            {
                return ContainerEnum.Food;
            }
        }

    }
}
