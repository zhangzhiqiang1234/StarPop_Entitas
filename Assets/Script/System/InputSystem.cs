using System;
using Entitas;

public class InputSystem : IExecuteSystem
{
    private Contexts _contexts;
    private IInputDrive _inputDrive;

    public InputSystem(Contexts contexts,IInputDrive input) : base()
    {
        _contexts = contexts;
        _inputDrive = input;
    }

    public void Execute()
    {
        if (_inputDrive.GetMouseButtonDown(0))
        {
            MVector3D vector3D = _inputDrive.MousePosition();
            int row;
            int col;
            ComputeRowAndCol(vector3D, out row, out col);
            if (_contexts.game.getEntityByRowAndCol(row,col) != null)
            {

                _contexts.game.CreateEntity().AddClickStar(row, col);
            }
        }
    }

    private void ComputeRowAndCol(MVector3D mousePos, out int row, out int col)
    {
        MVector3D worldPos = _inputDrive.ScreenToWorldPoint(mousePos);
        col = (int)Math.Floor(worldPos.x - (_contexts.game.GetStartPosX() - 0.5));
        row = (int)Math.Floor(worldPos.y - (_contexts.game.GetStartPosY() - 0.5));
    }
}
