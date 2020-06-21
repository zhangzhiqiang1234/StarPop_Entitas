using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class TimeComponent : IComponent
{
    public long FrameCount;
    public ITimeDrive TimeDrive;
}
