using System.Collections.Generic;
using Entitas;

public class OneFrameEventSystem : ReactiveSystem<GameEntity>
{
    public OneFrameEventSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.isDestroy = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasClickStar || entity.hasGainScore || entity.isCheckEnd;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        int[] allof = { GameComponentsLookup.ClickStar,GameComponentsLookup.GainScore,GameComponentsLookup.CheckEnd};
        return context.CreateCollector<GameEntity>(GameMatcher.AnyOf(allof));
    }
}
