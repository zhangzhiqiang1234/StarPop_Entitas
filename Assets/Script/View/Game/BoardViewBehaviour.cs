using Entitas.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class BoardViewBehaviour : MonoBehaviour
{
    private GameContext _context;
    private void Awake()
    {
        _context = Contexts.sharedInstance.game;
        EventManager.Instance.EventDispatcher.addEventListener<GameEntity>(EventEnum.FightUI_CreateView, this.addStarView);
        EventManager.Instance.EventDispatcher.addEventListener<int, int, int, int>(EventEnum.FightUI_Settlement, this.settlementPerform);
    }

    private void settlementPerform(int starNum, int totalScore, int preScore, int curScore)
    {
        StartCoroutine(deleteStarPerform(starNum,totalScore,preScore,curScore));
    }

    private IEnumerator deleteStarPerform(int starNum, int totalScore, int preScore, int curScore)
    {

        for (int i = starNum - 1; i >= 0; i--)
        {
            GameObject starObject = gameObject.transform.GetChild(i).gameObject;
            starObject.SetActive(false);
            totalScore = totalScore - preScore;
            totalScore = totalScore >= 0 ? totalScore : 0;
            //播放消失动画

            if (totalScore > 0)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
        //更新当前分数
        EventManager.Instance.EventDispatcher.dispatchEvent<int, float, float>(EventEnum.FightUI_Update_LevelInfo, -1, -1, curScore);
        ChangeLevelView changeLevelView = UIManager.Instance.ShowView<ChangeLevelView>(UIType.Fight_ChangeLevelView);
        if (changeLevelView)
        {
            LevelInfoComponent levelInfo = _context.levelInfo;
            IGameConfig gameConfig = _context.gameConfig.config;
            if (levelInfo != null && gameConfig != null)
            {
                changeLevelView.Show(levelInfo.curScore >= levelInfo.targetScore, levelInfo.curLevelId >= gameConfig.GetLevelTotalNum());
            }
        }
    }

    private void clearStarView()
    {
        for (int i = gameObject.transform.childCount - 1;  i >= 0; i--)
        {
            GameObject starObject = gameObject.transform.GetChild(i).gameObject;
            starObject.Unlink();
            GameObject.Destroy(starObject);
        }
    }

    private void addStarView(GameEntity entity)
    {
        string name = entity.asset.assetName;
        float x = entity.position.x;
        float y = entity.position.y;
        var prefab = Resources.Load<GameObject>(name);
        GameObject starObject = GameObject.Instantiate(prefab,this.gameObject.transform);
        IView view = starObject.GetComponent<IView>();
        view.Link(entity);
        starObject.transform.localPosition = new Vector3(x, y, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        EventManager.Instance.EventDispatcher.removeEventListener<GameEntity>(EventEnum.FightUI_CreateView, this.addStarView);
        EventManager.Instance.EventDispatcher.removeEventListener<int,int,int,int>(EventEnum.FightUI_Settlement, this.settlementPerform);
    }
}
