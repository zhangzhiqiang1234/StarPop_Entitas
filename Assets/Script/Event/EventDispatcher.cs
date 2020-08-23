
using System;
using System.Collections.Generic;

public delegate void CallBack();
public delegate void CallBack<T>(T t);
public delegate void CallBack<T,W>(T t,W w);
public delegate void CallBack<T,W,X>(T t, W w, X x);
public delegate void CallBack<T,W,X,Y>(T t, W w, X x, Y y);
public delegate void CallBack<T,W,X,Y,Z>(T t, W w, X x, Y y, Z z);

public class EventDispatcher
{
    private Dictionary<EventEnum, Delegate> _events = new Dictionary<EventEnum, Delegate>();

    private void checkAddListener(EventEnum eventName,Delegate callBack)
    {
        if(!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, null);
        }
        Delegate call = _events[eventName];
        if (call != null && call.GetType() != callBack.GetType())
        {
            throw new Exception(string.Format("事件{0},添加的事件类型不一致，原本的为{1},现在添加的为{2}", eventName, call.GetType(), callBack.GetType()));
        }
    }

    public void addEventListener(EventEnum eventName,CallBack callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack)_events[eventName] + callBack;
    }

    internal void addEventListener<T1, T2, T3>(EventEnum fightUI_Update_LevelInfo, Action<LevelData, T3> updateInfo)
    {
        throw new NotImplementedException();
    }

    public void addEventListener<T>(EventEnum eventName, CallBack<T> callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack<T>)_events[eventName] + callBack;
    }

    public void addEventListener<T, W>(EventEnum eventName, CallBack<T, W> callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack<T, W>)_events[eventName] + callBack;
    }

    public void addEventListener<T, W, X>(EventEnum eventName, CallBack<T, W, X> callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X>)_events[eventName] + callBack;
    }

    public void addEventListener<T, W, X, Y>(EventEnum eventName, CallBack<T, W, X, Y> callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X, Y>)_events[eventName] + callBack;
    }

    public void addEventListener<T, W, X, Y, Z>(EventEnum eventName, CallBack<T, W, X, Y, Z> callBack)
    {
        checkAddListener(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X, Y, Z>)_events[eventName] + callBack;
    }

    private void checkEventRevome(EventEnum eventName,Delegate callBack)
    {
        if(_events.ContainsKey(eventName))
        {
            Delegate call = _events[eventName];
            if(call == null)
            {
                throw new Exception(string.Format("{0}这个事件没有委托", eventName));
            }
            if(call.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("事件{0},移除的事件类型不一致，原本的为{1},现在添加的为{2}", eventName, call.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("没有{0}这个事件", eventName));
        }
    }

    public void removeEventListener(EventEnum eventName,CallBack callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack)_events[eventName] - callBack;
        if(_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    public void removeEventListener<T>(EventEnum eventName, CallBack<T> callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack<T>)_events[eventName] - callBack;
        if (_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    public void removeEventListener<T, W>(EventEnum eventName, CallBack<T, W> callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack<T, W>)_events[eventName] - callBack;
        if (_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    public void removeEventListener<T, W, X>(EventEnum eventName, CallBack<T, W, X> callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X>)_events[eventName] - callBack;
        if (_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    public void removeEventListener<T, W, X, Y>(EventEnum eventName, CallBack<T, W, X, Y> callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X, Y>)_events[eventName] - callBack;
        if (_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    public void removeEventListener<T, W, X, Y, Z>(EventEnum eventName, CallBack<T, W, X, Y, Z> callBack)
    {
        checkEventRevome(eventName, callBack);
        _events[eventName] = (CallBack<T, W, X, Y, Z>)_events[eventName] - callBack;
        if (_events[eventName] == null)
        {
            _events.Remove(eventName);
        }
    }

    private void checkDispatchEvent(EventEnum eventName)
    {
        if(_events.ContainsKey(eventName))
        {
            if(_events[eventName] == null)
            {
                throw new Exception(string.Format("{0}事件没有委托", eventName));
            }
        }
        else
        {
            throw new Exception(string.Format("没有添加{0}事件", eventName));
        }
    }

    public void dispatchEvent(EventEnum eventName)
    {
        checkDispatchEvent(eventName);
        CallBack callBack = (CallBack)_events[eventName];
        callBack();
    }

    public void dispatchEvent<T>(EventEnum eventName,T t)
    {
        checkDispatchEvent(eventName);
        CallBack<T> callBack = (CallBack<T>)_events[eventName];
        callBack(t);
    }

    public void dispatchEvent<T,W>(EventEnum eventName, T t, W w)
    {
        checkDispatchEvent(eventName);
        CallBack<T,W> callBack = (CallBack<T,W>)_events[eventName];
        callBack(t,w);
    }

    public void dispatchEvent<T, W, X, Y, Z>(EventEnum eventName, T t, W w, X x, Y y, Z z)
    {
        checkDispatchEvent(eventName);
        CallBack<T, W, X, Y, Z> callBack = (CallBack<T, W, X, Y, Z>)_events[eventName];
        callBack(t, w, x, y, z);
    }

    public void dispatchEvent<T, W, X, Y>(EventEnum eventName,T t, W w, X x, Y y)
    {
        checkDispatchEvent(eventName);
        CallBack<T, W, X, Y> callBack = (CallBack<T, W, X, Y>)_events[eventName];
        callBack(t, w, x, y);
    }

    public void dispatchEvent<T, W, X>(EventEnum eventName,T t, W w, X x)
    {
        checkDispatchEvent(eventName);
        CallBack<T, W, X> callBack = (CallBack<T, W, X>)_events[eventName];
        callBack(t, w, x);
    }
}
