using System.Collections.Generic;
using Entitas;

public class TimeSystem : IInitializeSystem,IExecuteSystem
{
    Contexts _contexts;
    ITimeDrive _timeDrive;

    public TimeSystem(Contexts contexts,ITimeDrive timeDrive)
    {
        _contexts = contexts;
        _timeDrive = timeDrive;
    }

    public void Initialize()
    {
        _contexts.game.ReplaceTime(0, _timeDrive);
    }

    public void Execute()
    {
        _contexts.game.ReplaceTime(++_contexts.game.time.FrameCount, _timeDrive);
    }
}
