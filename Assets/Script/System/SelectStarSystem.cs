using System.Collections.Generic;
using Entitas;

public class SelectStarSystem : ReactiveSystem<GameEntity>
{
    public SelectStarSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {

        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasSelectStar && !entity.isDestroy;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.SelectStar);
    }
}
