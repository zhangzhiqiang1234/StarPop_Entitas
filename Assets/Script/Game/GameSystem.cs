using Entitas;
/// <summary>
/// 添加所有的system
/// </summary>
public class GameSystem : Feature
{
    public GameSystem(Contexts contexts)
    {
        Add(new BoardSystem(contexts));
    }
}
