
//功能：
//创建者: 胡海辉
//创建时间：


using System;
using Assets.Script.Tools;
using UnityEngine;

namespace Assets.Script.EventMgr
{

    [Serializable]
    public class SceneBgm 
    {
      public int audioId;
    }
    /// <summary>
    /// 触发器
    /// </summary>
    public class ContainerTypeParam : EventArgs
    {
        public ContainerEnum ContainerType;
        public ContainerTypeParam() { }
        public ContainerTypeParam(ContainerEnum containerType)
        {
            ContainerType = containerType;
        }
    }


}
