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
            e.isDestroy = true;
            int levelId = e.loadLevel.levelId;
            LevelData data = _gameContext.gameConfig.config.GetLevelData(levelId);
            if (data != null)
            {
                _gameContext.InitBoradDatas(data.GetRow(),data.GetCol());
                int currentScore = 0;
                if (_gameContext.hasLevelInfo && levelId != 1)
                {
                    currentScore = _gameContext.levelInfo.curScore;
                }
                _gameContext.ReplaceLevelInfo(data.GetLevelId(), data.GetRow(), data.GetCol(), currentScore, data.GetEndTotalScore(),data.GetEndPreScore(),data.GetTargetScore());

                return;
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

