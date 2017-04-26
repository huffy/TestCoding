
//功能：
//创建者: 胡海辉
//创建时间：


using Assets.Script.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.EventMgr
{
    public class EventHelpr : TSingleton<EventHelpr>, IDisposable
    {
        public override void Dispose()
        {
            base.Dispose();
        }
    }

    public delegate void TDelegate();
    public delegate void TDelegate<T>(T t);
    public delegate void TDelegate<T0,T1>(T0 t0,T1 t1);
    public delegate void TDelegate<T0, T1, T2>(T0 t0, T1 t1, T2 t2);
}
