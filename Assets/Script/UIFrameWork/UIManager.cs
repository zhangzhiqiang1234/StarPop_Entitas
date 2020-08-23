using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    /// <summary>
    /// UI管理类
    /// UI层级分为三层:
    /// 固定层:FixedLayer 用于最低层的界面，一般用于主界面，战斗界面等，同时只能存在一个固定界面，
    ///        加入一个新的固定界面时会将其它两个层的界面都移除。
    /// 弹窗层:WindowLayer 大部分界面都在这个层，可以出现重复的界面
    /// 提示层:TipsLayer 提示消息放在最顶层
    /// </summary>
    public class UIManager
    {
        private static UIManager _Instance;

        private long _instanceViewID;
        private long _curTopViewID;

        private Dictionary<UIType, Stack<BaseView>> _dicCacheView;
        private Dictionary<UILayer, List<BaseView>> _dicOpenView;
        //打开顺序集合
        private List<BaseView> _currentViewList;

        private Dictionary<UIType, UIConfigData> _dicUIConfigs;

        private Transform _fixedLayer;
        private Transform _windowLayer;
        private Transform _tipsLayer;


        public static UIManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UIManager();
                    _Instance.InitRootUI();
                }
                return _Instance;
            }   
        }

        private T LoadUIPrefab<T>(string path) where T : UnityEngine.Object
        {
            T prefab = Resources.Load<T>(path);
            return GameObject.Instantiate<T>(prefab);
        }


        private void InitRootUI()
        {
            GameObject uiRoot = LoadUIPrefab<GameObject>("UIPrefabs/UIRoot");
            GameObject.DontDestroyOnLoad(uiRoot);

            _fixedLayer = uiRoot.transform.Find("Canvas/FixedLayer");
            _windowLayer = uiRoot.transform.Find("Canvas/WindowLayer");
            _tipsLayer = uiRoot.transform.Find("Canvas/TipsLayer");

            _dicCacheView = new Dictionary<UIType, Stack<BaseView>>();
            _dicOpenView = new Dictionary<UILayer, List<BaseView>>();
            _currentViewList = new List<BaseView>();
            _dicUIConfigs = new Dictionary<UIType, UIConfigData>();

            _instanceViewID = 0;
            _curTopViewID = -1;

            InitUIConfigs();
        }

        private void InitUIConfigs()
        {
            string datastr = Resources.Load<TextAsset>("UIConfig/UIConfig").text;
            UIConfigDatas datas = JsonUtility.FromJson<UIConfigDatas>(datastr);
            for (int i = 0; i < datas.Datas.Length; i++)
            {
                UIConfigData data = datas.Datas[i];
                _dicUIConfigs.Add((UIType)data.uiType, data);
            }
        }


        /// <summary>
        /// 显示UI界面
        /// <param name="uiType">界面类型</param>
        /// <param name="uiLayer">UI的属于哪个层级</param>
        /// <param name="isReapt">是否可以同时显示多个类型相同的界面</param>
        /// <returns>BaseView</returns>
        public T ShowView<T>(UIType uiType, bool isReapt = false)
        {
            return (T)(object)CreateView(uiType, isReapt);
        }

        /// <summary>
        /// 显示UI界面
        /// <param name="uiType">界面类型</param>
        /// <param name="uiLayer">UI的属于哪个层级</param>
        /// <param name="isReapt">是否可以同时显示多个类型相同的界面</param>
        /// <returns></returns>
        public void ShowView(UIType uiType, bool isReapt = false)
        {
            CreateView(uiType, isReapt);
        }

        private BaseView CreateView(UIType uiType, bool isReapt = false)
        {
            if (!_dicUIConfigs.ContainsKey(uiType))
            {
                Debug.LogError(string.Format("请在UIType={0},是否在UIConfig.json中配置！", uiType));
                return null;
            }

            BaseView curView;
            UIConfigData uiConfig = _dicUIConfigs[uiType];

            //1、判断是否时可重复的View
            if (!isReapt)
            {
                //如果不是可重复界面，那么将当前打开的这个类型的界面全都关闭
                CloseView(uiType, true);
            }

            //2、判断打开的是不是固定界面
            if ((UILayer)uiConfig.uiLayer == UILayer.Fixed)
            {
                CloseAllView();
            }

            //3、判断是否有缓存
            curView = PopViewFromCache(uiType);  
            if (curView == null)
            {
                //创建新的View
                
                Transform parent = GetChildLayer((UILayer)uiConfig.uiLayer);
                GameObject prefab = Resources.Load<GameObject>(uiConfig.prefabPath);
                curView = GameObject.Instantiate<GameObject>(prefab, parent, false).GetComponent<BaseView>();
            }

            //4、生成新的ViewId，添加界面  
            curView.InitViewAttribute(++_instanceViewID, uiConfig);
            AddChildToLayer(curView);

            //5、更新_curTopViewID
            BaseView topView = GetTopView();
            if (topView)
            {
                _curTopViewID = topView.GetViewID();
            }
            return curView;
        }

        /// <summary>
        /// 关闭UI界面
        /// </summary>
        /// <param name="viewid">界面的唯一ID</param>
        public void CloseView(long viewid)
        {
            for (int i = _currentViewList.Count - 1; i >= 0; i--)
            {
                if (_currentViewList[i].GetViewID() == viewid)
                {
                    PushViewToCache(_currentViewList[i]);
                    RemoveChildFromLayer(_currentViewList[i]);
                    break;
                }
            }
        }

        /// <summary>
        /// 关闭UI界面
        /// </summary>
        /// <param name="uiType">界面类型</param>
        /// <param name="closeAll">是否将当前类型的界面全部关闭，如果为false,则关闭最上面显示的当前类型的界面</param>
        public void CloseView(UIType uiType, bool closeAll = false)
        {
            for (int i = _currentViewList.Count - 1; i >= 0; i--)
            {
                if (_currentViewList[i].GetUIType() == uiType)
                {
                    PushViewToCache(_currentViewList[i]);
                    RemoveChildFromLayer(_currentViewList[i]);
                    if (!closeAll)
                    {
                        break;
                    }
                }
            }
        }

        private void CloseAllView()
        {
            for (int i = _currentViewList.Count - 1; i >= 0; i--)
            {
                PushViewToCache(_currentViewList[i]);
                RemoveChildFromLayer(_currentViewList[i]);
            }
        }

        /// <summary>
        /// 获取顶层界面
        /// </summary>
        /// <returns></returns>
        private BaseView GetTopView()
        {
            BaseView topView = null;

            if (_dicOpenView.ContainsKey(UILayer.Window) && _dicOpenView[UILayer.Window].Count > 0)
            {
                topView = _dicOpenView[UILayer.Window][_dicOpenView[UILayer.Window].Count - 1];
                return topView;
            }

            if (_dicOpenView.ContainsKey(UILayer.Fixed) && _dicOpenView[UILayer.Fixed].Count > 0)
            {
                topView = _dicOpenView[UILayer.Fixed][_dicOpenView[UILayer.Fixed].Count - 1];
                return topView;
            }
            return topView;
        }

        /// <summary>
        /// 获得对应的层级Transform
        /// </summary>
        /// <param name="uiLayer"></param>
        /// <returns></returns>
        private Transform GetChildLayer(UILayer uiLayer)
        {
            Transform layer = null;
            switch (uiLayer)
            {
                case UILayer.Fixed:
                    layer = _fixedLayer;
                    break;
                case UILayer.Window:
                    layer = _windowLayer;
                    break;
                case UILayer.Tips:
                    layer = _tipsLayer;
                    break;
                default:
                    Debug.LogError("请检查UILayer的值是否正确!");
                    break;
            }
            return layer;
        }

        /// <summary>
        /// 将界面添加到对应的显示层
        /// </summary>
        /// <param name="baseView"></param>
        private void AddChildToLayer(BaseView baseView)
        {
            UILayer uiLayer = baseView.GetUILayer();
            Transform parent = GetChildLayer(uiLayer);
            if (parent)
            {
                if (baseView.transform.parent == null)
                {
                    baseView.transform.parent = parent;
                }
                baseView.gameObject.SetActive(true);

                //添加到对应集合中
                _currentViewList.Add(baseView);
                if (!_dicOpenView.ContainsKey(uiLayer))
                {
                    _dicOpenView.Add(uiLayer, new List<BaseView>());
                }
                _dicOpenView[uiLayer].Add(baseView);
            }
        }

        /// <summary>
        /// 将界面从显示层移除
        /// </summary>
        /// <param name="baseView"></param>
        private void RemoveChildFromLayer(BaseView baseView)
        {
            UILayer uiLayer = baseView.GetUILayer();

            _currentViewList.Remove(baseView);
            if (_dicOpenView.ContainsKey(uiLayer))
            {
                _dicOpenView[uiLayer].Remove(baseView);
            }

            baseView.transform.parent = null;
            baseView.gameObject.SetActive(false);
        }

        /// <summary>
        /// 加入缓存
        /// </summary>
        /// <param name="baseView"></param>
        private void PushViewToCache(BaseView baseView)
        {
            UIType uiType = baseView.GetUIType();
            if (!_dicCacheView.ContainsKey(uiType))
            {
                _dicCacheView.Add(uiType, new Stack<BaseView>());
            }
            _dicCacheView[uiType].Push(baseView);
        }

        /// <summary>
        /// 移出缓存
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        private BaseView PopViewFromCache(UIType uiType)
        {
            BaseView view = null;
            if (_dicCacheView.ContainsKey(uiType))
            {
                if (_dicCacheView[uiType].Count > 0)
                {
                    view = _dicCacheView[uiType].Pop();
                }
            }
            return view;
        }
    }
}
