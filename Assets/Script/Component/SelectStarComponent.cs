using Entitas;
using Entitas.CodeGeneration.Attributes;

[Event(EventTarget.Self)]
public class SelectStarComponent : IComponent
{
    public bool isSelect;
    public bool showUpEdge;
    public bool showDownEdge;
    public bool showLeftEdge;
    public bool showRightEdge;
}
