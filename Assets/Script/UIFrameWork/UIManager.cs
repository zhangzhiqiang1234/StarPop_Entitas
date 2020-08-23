using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrameWork
{
    /// <summary>
    /// UI������
    /// UI�㼶��Ϊ����:
    /// �̶���:FixedLayer ������Ͳ�Ľ��棬һ�����������棬ս������ȣ�ͬʱֻ�ܴ���һ���̶����棬
    ///        ����һ���µĹ̶�����ʱ�Ὣ����������Ľ��涼�Ƴ���
    /// ������:WindowLayer �󲿷ֽ��涼������㣬���Գ����ظ��Ľ���
    /// ��ʾ��:TipsLayer ��ʾ��Ϣ�������
    /// </summary>
    public class UIManager
    {
        private static UIManager _Instance;

        private long _instanceViewID;
        private long _curTopViewID;

        private Dictionary<UIType, Stack<BaseView>> _dicCacheView;
        private Dictionary<UILayer, List<BaseView>> _dicOpenView;
        //��˳�򼯺�
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
        /// ��ʾUI����
        /// <param name="uiType">��������</param>
        /// <param name="uiLayer">UI�������ĸ��㼶</param>
        /// <param name="isReapt">�Ƿ����ͬʱ��ʾ���������ͬ�Ľ���</param>
        /// <returns>BaseView</returns>
        public T ShowView<T>(UIType uiType, bool isReapt = false)
        {
            return (T)(object)CreateView(uiType, isReapt);
        }

        /// <summary>
        /// ��ʾUI����
        /// <param name="uiType">��������</param>
        /// <param name="uiLayer">UI�������ĸ��㼶</param>
        /// <param name="isReapt">�Ƿ����ͬʱ��ʾ���������ͬ�Ľ���</param>
        /// <returns></returns>
        public void ShowView(UIType uiType, bool isReapt = false)
        {
            CreateView(uiType, isReapt);
        }

        private BaseView CreateView(UIType uiType, bool isReapt = false)
        {
            if (!_dicUIConfigs.ContainsKey(uiType))
            {
                Debug.LogError(string.Format("����UIType={0},�Ƿ���UIConfig.json�����ã�", uiType));
                return null;
            }

            BaseView curView;
            UIConfigData uiConfig = _dicUIConfigs[uiType];

            //1���ж��Ƿ�ʱ���ظ���View
            if (!isReapt)
            {
                //������ǿ��ظ����棬��ô����ǰ�򿪵�������͵Ľ���ȫ���ر�
                CloseView(uiType, true);
            }

            //2���жϴ򿪵��ǲ��ǹ̶�����
            if ((UILayer)uiConfig.uiLayer == UILayer.Fixed)
            {
                CloseAllView();
            }

            //3���ж��Ƿ��л���
            curView = PopViewFromCache(uiType);  
            if (curView == null)
            {
                //�����µ�View
                
                Transform parent = GetChildLayer((UILayer)uiConfig.uiLayer);
                GameObject prefab = Resources.Load<GameObject>(uiConfig.prefabPath);
                curView = GameObject.Instantiate<GameObject>(prefab, parent, false).GetComponent<BaseView>();
            }

            //4�������µ�ViewId����ӽ���  
            curView.InitViewAttribute(++_instanceViewID, uiConfig);
            AddChildToLayer(curView);

            //5������_curTopViewID
            BaseView topView = GetTopView();
            if (topView)
            {
                _curTopViewID = topView.GetViewID();
            }
            return curView;
        }

        /// <summary>
        /// �ر�UI����
        /// </summary>
        /// <param name="viewid">�����ΨһID</param>
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
        /// �ر�UI����
        /// </summary>
        /// <param name="uiType">��������</param>
        /// <param name="closeAll">�Ƿ񽫵�ǰ���͵Ľ���ȫ���رգ����Ϊfalse,��ر���������ʾ�ĵ�ǰ���͵Ľ���</param>
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
        /// ��ȡ�������
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
        /// ��ö�Ӧ�Ĳ㼶Transform
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
                    Debug.LogError("����UILayer��ֵ�Ƿ���ȷ!");
                    break;
            }
            return layer;
        }

        /// <summary>
        /// ��������ӵ���Ӧ����ʾ��
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

                //��ӵ���Ӧ������
                _currentViewList.Add(baseView);
                if (!_dicOpenView.ContainsKey(uiLayer))
                {
                    _dicOpenView.Add(uiLayer, new List<BaseView>());
                }
                _dicOpenView[uiLayer].Add(baseView);
            }
        }

        /// <summary>
        /// ���������ʾ���Ƴ�
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
        /// ���뻺��
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
        /// �Ƴ�����
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
