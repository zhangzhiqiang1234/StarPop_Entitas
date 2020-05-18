using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            e.AddComponent(GameComponentsLookup.View,new ViewComponent());
            //发送事件，创建视图
            _contexts.game.eventDispatcher.dispatchEvent<GameEntity>(EventEnum.FIGHT_CREATEVIEW, e);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasAsset && !entity.HasComponent(GameComponentsLookup.View);
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.Asset);
    }
}