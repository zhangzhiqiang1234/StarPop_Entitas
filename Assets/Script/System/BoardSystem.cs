using Entitas;

public class BoardSystem : IInitializeSystem
{
    GameContext _gameContext;

    public BoardSystem(Contexts contexts)
    {
        _gameContext = contexts.game;
    }

    public void Initialize()
    {
        _gameContext.InitBoradDatas(_gameContext.gameConfig.config.GetBoardRow(), _gameContext.gameConfig.config.GetBoardCol());
    }
}

