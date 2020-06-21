using System.Collections.Generic;
using Entitas;

public class LogSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    ILogDrive _logDrive;
    public LogSystem(Contexts contexts,ILogDrive log) : base(contexts.game)
    {
        _contexts = contexts;
        _logDrive = log;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            _logDrive.LogMessage(e.log.message);
            e.isDestroy = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLog;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.Log);
    }
}
