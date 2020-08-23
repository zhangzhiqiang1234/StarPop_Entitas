using UIFrameWork;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeLevelView : BaseView
{
    private Text txtTip;
    private Button btnContinue;
    private Button btnExit;

    protected override void OnEnter()
    {
        InitViews();
    }

    public void Show(bool isWin,bool isLastLevel)
    {
        string tipstr = "";
        if (isWin)
        {
            if (isLastLevel)
            {
                tipstr = "恭喜大神，通过了所有关卡！";
            }
            else
            {
                tipstr = "你真厉害，快开始下一关吧！";
            }
        }
        else
        {
            tipstr = "真遗憾，就差一点点了，继续努力吧！";
        }
        txtTip.text = tipstr;
    }

    private void InitViews()
    {
        txtTip = transform.Find("txt_Tips").GetComponent<Text>();
        btnContinue = transform.Find("btn_Continue").GetComponent<Button>();
        btnExit = transform.Find("btn_Exit").GetComponent<Button>();

        UIEventTriggerListener.GetListener(btnContinue.gameObject).onClick += onContinueClick;
        UIEventTriggerListener.GetListener(btnExit.gameObject).onClick += onExitClick;
    }

    private void onExitClick(GameObject gameObject, PointerEventData eventData)
    {
        EventManager.Instance.EventDispatcher.dispatchEvent<bool>(EventEnum.Fight_ChangeLevel,true);
        UIManager.Instance.CloseView(GetViewID());
    }

    private void onContinueClick(GameObject gameObject, PointerEventData eventData)
    {
        EventManager.Instance.EventDispatcher.dispatchEvent<bool>(EventEnum.Fight_ChangeLevel,false);
        UIManager.Instance.CloseView(GetViewID());
    }
}
