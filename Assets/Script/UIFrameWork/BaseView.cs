using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    public class BaseView : MonoBehaviour
    {
        private long _viewID;
        private UIConfigData _configData;

        #region UnityMethod

        private void Awake()
        {
            OnEnter();
        }

        private void OnDestroy()
        {
            OnExit();
        }

        private void OnEnable()
        {
            OnActive();
        }

        private void OnDisable()
        {
            OnInactive();
        }

        #endregion


        #region SetAttribute

        private void SetViewID(long id)
        {
            _viewID = id;
        }

        private void SetUIConfigData(UIConfigData data)
        {
            _configData = data;
        }

        public long GetViewID()
        {
            return _viewID;
        }

        public UIType GetUIType()
        {
            return (UIType)_configData.uiType;
        }

        public UILayer GetUILayer()
        {
            return (UILayer)_configData.uiLayer;
        }

        public void InitViewAttribute(long id,UIConfigData data)
        {
            SetViewID(id);
            SetUIConfigData(data);
        }

        #endregion

        #region VirtualMethod

        protected virtual void OnEnter(){ }
        protected virtual void OnExit() { }
        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }
        public virtual void ShowOnTop() { }

        #endregion
    }
}
