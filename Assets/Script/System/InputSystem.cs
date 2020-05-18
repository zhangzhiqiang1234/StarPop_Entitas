using System;
using Entitas;
using UnityEngine;

public class InputSystem : IExecuteSystem
{
    private Contexts _contexts;

    public InputSystem(Contexts contexts) : base()
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            int row;
            int col;
            ComputeRowAndCol(mousePos, out row, out col);
            if(row >= 0 && row <= _contexts.game.gameConfig.config.GetBoardRow() && 
                col >= 0 && col <= _contexts.game.gameConfig.config.GetBoardCol())
            {
                Debug.Log(string.Format("row {0} col {1}", row, col));
            }

        }
    }

    private void ComputeRowAndCol(Vector3 mousePos, out int row, out int col)
    {
        Vector3 worldPos =  Camera.main.ScreenToWorldPoint(mousePos);
        col = (int)Math.Floor(worldPos.x - (_contexts.game.GetStartPosX() - 0.5));
        row = (int)Math.Floor(worldPos.y - (_contexts.game.GetStartPosY() - 0.5));
    }
}
