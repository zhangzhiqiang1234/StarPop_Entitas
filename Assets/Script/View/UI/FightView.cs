using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class FightView : BaseView
{
    private Text txtLevel;
    private Text txtTargetScore;
    private Text txtCurrenttScore;


    public override void ShowOnTop()
    {
        
    }

    protected override void OnActive()
    {
        InitEvents();
    }

    protected override void OnEnter()
    {
        InitViews();
    }

    protected override void OnExit()
    {
        
    }

    protected override void OnInactive()
    {
        RemoveEvents();
    }

    private void InitViews()
    {
        txtLevel = transform.Find("txtLevel").GetComponent<Text>();
        txtTargetScore = transform.Find("txtTargetScore").GetComponent<Text>();
        txtCurrenttScore = transform.Find("txtCurrenttScore").GetComponent<Text>();
    }

    private void InitEvents()
    {
        EventManager.Instance.EventDispatcher.addEventListener<int, float, float>(EventEnum.FightUI_Update_LevelInfo, this.UpdateInfo);
    }

    private void RemoveEvents()
    {
        EventManager.Instance.EventDispatcher.removeEventListener<int, float, float>(EventEnum.FightUI_Update_LevelInfo, this.UpdateInfo);
    }

    private void UpdateInfo(int levelID, float targetScore, float currentScore)
    {
        txtLevel.text = levelID > 0 ? levelID.ToString() : txtLevel.text;
        txtTargetScore.text = targetScore >= 0 ? targetScore.ToString() : txtTargetScore.text;
        txtCurrenttScore.text = currentScore >= 0 ? currentScore.ToString() : txtCurrenttScore.text;
    }
}
