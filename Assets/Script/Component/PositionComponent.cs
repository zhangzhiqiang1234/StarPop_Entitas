using Entitas;
using Entitas.CodeGeneration.Attributes;

[Event(EventTarget.Self)]
public class PositionComponent : IComponent
{
    public float x;
    public float y;
}