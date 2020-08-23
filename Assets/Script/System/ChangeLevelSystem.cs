using System;
using System.Collections.Generic;
using Entitas;

public class ChangeLevelSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    private Group<GameEntity> _group;

    public ChangeLevelSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
        _group = (Group<GameEntity>)_contexts.game.GetGroup(GameMatcher.Star);
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var item in entities)
        {
            clearStarEntity();
            loadNextLevel(item.changeLevel.isExit);
            item.isDestroy = true;
            break;
        }
    }

    private void loadNextLevel(bool isExit)
    {
        //逻辑下一关
        GameEntity entity = _contexts.game.CreateEntity();
        int levelId = 1;
        if (!isExit)
        {
            if (_contexts.game.levelInfo.curScore >= _contexts.game.levelInfo.targetScore)
            {
                levelId = _contexts.game.levelInfo.curLevelId + 1;
                if (levelId > _contexts.game.gameConfig.config.GetLevelTotalNum())
                {
                    levelId = 1;
                }
            }
        }
        entity.AddLoadLevel(levelId);
    }

    private void clearStarEntity()
    {
        for (int i = 0; i < _group.GetEntities().Length; i++)
        {
            GameEntity e = _group.GetEntities()[i];
            _contexts.game.ClearData(e.star.rowNum, e.star.colNum);
            e.isDestroy = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasChangeLevel;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.ChangeLevel);
    }
}
