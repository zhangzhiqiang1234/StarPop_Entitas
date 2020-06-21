using Entitas;
/// <summary>
/// 添加所有的system
/// </summary>
public class GameSystem : Feature
{
    public GameSystem(Contexts contexts,DrivesEntry drives)
    {
        Add(new TimeSystem(contexts, drives.Time));

        Add(new BoardSystem(contexts));
        Add(new AddViewSystem(contexts));

        Add(new InputSystem(contexts,drives.Input));

        Add(new SelectStarSystem(contexts));

        Add(new DestroyStarsSystem(contexts));
        Add(new GainScoreSystem(contexts));
        Add(new ResultJudgeSystem(contexts));

        Add(new GameEventSystems(contexts));

        Add(new OneFrameEventSystem(contexts));
        Add(new LogSystem(contexts, drives.Log));
        Add(new GameCleanUpSystem(contexts));
    }
}
