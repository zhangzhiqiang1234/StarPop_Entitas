using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventTriggerListener : EventTrigger
{
    //点击的时间
    private float clickTime;
    //是否在按下时回调方法
    private bool isCallBackInDown = false;
    //长按的时间，单位是秒
    private float longClickTime = 1.0f;
    //触摸滑出区域外是否结束触摸
    private bool isOverTouchInOutside = false;


    private bool inCurrentClick = false;
    private bool inCurrentTouch = false;

    #region GetListener方法
    public static UIEventTriggerListener GetListener(GameObject gameObject)
    {
        UIEventTriggerListener uiEvent = gameObject.GetComponent<UIEventTriggerListener>();
        if (uiEvent == null)
        {
            uiEvent = gameObject.AddComponent<UIEventTriggerListener>();
        }
        return uiEvent;
    }

    #endregion

    #region Delegate方法
    public delegate void UIPointerEventDelegate(GameObject gameObject, PointerEventData eventData);
    #endregion

    #region UIEvent方法
    /// <summary>
    /// 点击事件
    /// </summary>
    public UIPointerEventDelegate onClick;
    /// <summary>
    /// 长按事件
    /// </summary>
    /// <returns></returns>
    public UIPointerEventDelegate onLongClick;
    /// <summary>
    /// 触摸开始事件
    /// </summary>
    public UIPointerEventDelegate onTouchBegin;
    /// <summary>
    /// 触摸移动事件
    /// </summary>
    public UIPointerEventDelegate onTouchMoved;
    /// <summary>
    /// 触摸结束事件
    /// </summary>
    public UIPointerEventDelegate onTouchEnded;
    #endregion

    #region 点击操作,如果点击与长按都是存在的话,优先响应长按事件
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        clickTime = Time.unscaledTime;
        inCurrentClick = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (inCurrentClick)
        {
            float clickDeltaTime = Time.unscaledTime - clickTime;
            if (clickDeltaTime >= longClickTime && onLongClick != null)
            {
                onLongClick(gameObject, eventData);
            }
            else
            {
                if (onClick != null)
                {
                    onClick(gameObject, eventData);
                }
            }
            inCurrentClick = false;
        }
    }
    #endregion

    #region
    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        base.OnInitializePotentialDrag(eventData);
        inCurrentTouch = true;
        if (onTouchBegin != null)
        {
            onTouchBegin(gameObject,eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (inCurrentTouch && onTouchMoved != null)
        {
            onTouchMoved(gameObject, eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        if (inCurrentTouch && onTouchEnded != null)
        {
            onTouchEnded(gameObject, eventData);
        }
        inCurrentTouch = false;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        if (isOverTouchInOutside)
        {
            OnEndDrag(eventData);
        }
    }

    #endregion

    private void Update()
    {
        if (inCurrentClick && isCallBackInDown)
        {
            float clickDeltaTime = Time.unscaledTime - clickTime;
            if (onLongClick != null)
            {
                if (clickDeltaTime >= longClickTime)
                {
                    inCurrentClick = false;
                    onLongClick(gameObject, null);
                }
            }
            else
            {
                if (onClick != null)
                {
                    inCurrentClick = false;
                    onClick(gameObject, null);
                }
            }
        }
    }

    public UIEventTriggerListener SetCallBackInDown(bool inDown)
    {
        isCallBackInDown = inDown;
        return this;
    }

    public UIEventTriggerListener SetLongTouchTime(float time)
    {
        longClickTime = time > 0 ? time : 0;
        return this;
    }

    public UIEventTriggerListener SetOverTouchInOutside(bool inOutside)
    {
        isOverTouchInOutside = inOutside;
        return this;
    }
}
