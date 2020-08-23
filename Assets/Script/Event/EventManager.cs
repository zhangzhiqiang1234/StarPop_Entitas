public class EventManager
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventManager();
            }
            return _instance;
        }   
    }

    private EventDispatcher _eventDispatcher;
    public EventDispatcher EventDispatcher { get => _eventDispatcher;}

    private EventManager()
    {
        _eventDispatcher = new EventDispatcher();
    }
}
