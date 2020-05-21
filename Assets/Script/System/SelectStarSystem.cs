using System.Collections.Generic;
using Entitas;

public class SelectStarSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    public SelectStarSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 0)
        {
            GameEntity selectEntity = entities[0];

            List<GameEntity> resultList = _contexts.game.DepthSearch(selectEntity.clickStar.row,selectEntity.clickStar.col);
            foreach (GameEntity entity in resultList)
            {
                _contexts.game.ClearData(entity.star.rowNum, entity.star.colNum);
                entity.isDestroy = true;
            }
            selectEntity.isDestroy = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasClickStar && !entity.isDestroy;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.ClickStar);
    }
}
