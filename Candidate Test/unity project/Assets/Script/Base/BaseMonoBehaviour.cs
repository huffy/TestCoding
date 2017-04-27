
//功能：mono基类
//创建者: 胡海辉
//创建时间：


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Script.Base
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        // cache gameobject 
        public GameObject CacheObj
        {
            get;
            private set;
        }

        // cache transform 
        public Transform CacheTrans
        {
            get;
            private set;
        }

        public virtual void Awake()
        {
            CacheObj = gameObject;
            CacheTrans = transform;
            Init();
        }

        public virtual void Update()
        {
        }

        public virtual void LateUpdate()
        {
        }

        public virtual void OnDestroy()
        {
            RemoveListener();
        }

        public virtual void Init()
        {
            InitComponent();
            InitData();
            InitListener();
        }

        public virtual void InitComponent()
        {
        }

        public virtual void InitData()
        {

        }

        /// <summary>
        /// 初始化监听
        /// </summary>
        public virtual void InitListener()
        {
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public virtual void RemoveListener()
        {
        }


    }
}
