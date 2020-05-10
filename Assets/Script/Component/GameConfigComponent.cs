using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class GameConfigComponent : IComponent
{
    public IGameConfig config;
}
