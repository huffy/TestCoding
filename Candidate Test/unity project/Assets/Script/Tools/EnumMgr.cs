using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Tools
{
    public enum GameStateEnum
    {
        None,
        Intro,
        PlayGame,
        End,
    }

    public enum BinAnimationEnum
    {
        BinClose,
        BinOpen,
        BinRollIn,
    }

    public enum ContainerEnum
    {
        Food,
        Glass,
        Paper,
        Plastic,
        Max,
    }

    public enum EventDefineEnum
    {
       SelectTrash,
       ReleaseTrash,     
    }

    public enum SoundEnum
    {
        PickUpTrash,
        EndMagicParticle,
        OpenBin,
        CloseBin,
        Trashbinmoving,
        TrashintheBinFood,
        TrashintheBinPlastics,
        TrashintheBinPaper,
        TrashintheBinGlass,
    }
}
