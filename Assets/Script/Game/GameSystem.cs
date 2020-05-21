using Entitas;
/// <summary>
/// 添加所有的system
/// </summary>
public class GameSystem : Feature
{
    public GameSystem(Contexts contexts)
    {
        Add(new BoardSystem(contexts));
        Add(new AddViewSystem(contexts));

        Add(new InputSystem(contexts));

        Add(new SelectStarSystem(contexts));

        Add(new GameEventSystems(contexts));

        Add(new GameCleanUpSystem(contexts));


    }
}
