using UnityEngine;
using System.Collections;
using System;
using Assets.Script.Base;
using UnityEngine.Events;
using System.Collections.Generic;
using Assets.Script.Tools;
namespace Assets.Script.EventMgr
{
    public class EventManager : TSingleton<EventManager>, IDisposable
    {
        private Dictionary<EventDefineEnum, UnityAction<object, EventArgs>> mEventDic = new Dictionary<EventDefineEnum, UnityAction<object, EventArgs>>();

        public EventManager()
        {
            if (mEventDic == null)
            {
                mEventDic = new Dictionary<EventDefineEnum, UnityAction<object, EventArgs>>();
                mEventDic.Clear();
            }
        }

        public void AddListener(EventDefineEnum eventID, UnityAction<object, EventArgs> eventHadle)
        {
            if (!mEventDic.ContainsKey(eventID))
            {
                mEventDic.Add(eventID, eventHadle);
            }
            else
            {
                mEventDic[eventID] += eventHadle;
            }
        }

        public void RemoveListener(EventDefineEnum eventID, UnityAction<object, EventArgs> eventHadle)
        {
            if (mEventDic.ContainsKey(eventID))
            {
                mEventDic[eventID] -= eventHadle;
            }
        }


        public void RasieEvent(EventDefineEnum eventID, object obj, EventArgs e)
        {
            if (mEventDic.ContainsKey(eventID))
            {
                mEventDic[eventID].Invoke(obj, e);
            }
        }

        public void RemoveAllListener()
        {
            if (mEventDic != null)
            {
                mEventDic.Clear();
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            RemoveAllListener();
            mEventDic = null;
        }

    }

}
