using Entitas;

/// <summary>
/// Entitas与Unity交互的入口
/// </summary>
public class GameController
{
    Systems _systems;
    public GameController(IGameConfig gameConfig)
    {
        _systems = new GameSystem(Contexts.sharedInstance);

        gameConfig.LoadLevelData("LevelData/LevelData");
        Contexts.sharedInstance.game.SetGameConfig(gameConfig);

        GameEntity entity = Contexts.sharedInstance.game.CreateEntity();
        entity.AddLoadLevel(1);
        entity.isDestroy = true;

    }

    /// <summary>
    /// 系统初始化函数
    /// </summary>
    public void Initialize()
    {
        _systems.Initialize();
    }

    public void Update()
    {
        _systems.Execute();
        _systems.Cleanup();
    }

}
