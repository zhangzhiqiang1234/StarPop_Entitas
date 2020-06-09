using System.Collections.Generic;
using Entitas;

public class BoardSystem : ReactiveSystem<GameEntity>
{
    GameContext _gameContext;

    public BoardSystem(Contexts contexts) : base(contexts.game)
    {
        _gameContext = contexts.game;
    }

    //public void Initialize()
    //{
    //    _gameContext.InitBoradDatas(_gameContext.gameConfig.config.GetBoardRow(), _gameContext.gameConfig.config.GetBoardCol());
    //}

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            int levelId = e.loadLevel.levelId;
            LevelData data = _gameContext.gameConfig.config.GetLevelData(levelId);
            if (data != null)
            {
                _gameContext.InitBoradDatas(data.GetRow(),data.GetCol());
                _gameContext.ReplaceLevelInfo(data.GetLevelId(), data.GetRow(), data.GetCol(), 0, data.GetTargetScore());

                break;
            }

        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasLoadLevel;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.LoadLevel);
    }
}

