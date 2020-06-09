using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique]
public class LevelInfoComponent : IComponent
{
    public int curLevelId;
    public int boardRow;
    public int boardCol;
    public int curScore;
    public int targetScore;
}
