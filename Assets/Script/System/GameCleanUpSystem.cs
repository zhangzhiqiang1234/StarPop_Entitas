using Entitas;
using System.Collections.Generic;

public sealed class GameCleanUpSystem : ICleanupSystem
{
    IGroup<GameEntity> _group;
    List<GameEntity> _buff = new List<GameEntity>();
    public GameCleanUpSystem(Contexts contexts) : base()
    {
        _group = contexts.game.GetGroup(GameMatcher.Destroy);
    }

    public void Cleanup()
    {
        foreach (var e in _group.GetEntities(_buff))
        {
            e.Destroy();
        }
    }
}
