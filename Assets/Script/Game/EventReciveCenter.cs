public class EventReciveCenter
{
    Contexts _contexts;
    public EventReciveCenter(Contexts contexts)
    {
        _contexts = contexts;
        AddEvents();
    }
    
    private void AddEvents()
    {
        EventManager.Instance.EventDispatcher.addEventListener<bool>(EventEnum.Fight_ChangeLevel, this.ChangeLevel);
    }

    private void RemoveEvents()
    {
        EventManager.Instance.EventDispatcher.removeEventListener<bool>(EventEnum.Fight_ChangeLevel, this.ChangeLevel);
    }

    ~EventReciveCenter()
    {
        RemoveEvents();
    }

    #region 事件方法
    private void ChangeLevel(bool isExit)
    {
        GameEntity gameEntity = _contexts.game.CreateEntity();
        gameEntity.AddChangeLevel(isExit);
    }

    #endregion
}
