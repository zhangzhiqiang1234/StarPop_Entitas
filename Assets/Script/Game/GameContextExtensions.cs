using UnityEngine;

public partial class GameContext
{
    private int[,] _boardDatas;

    public void InitBoradDatas(int row,int col)
    {
        _boardDatas = new int[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int starType = gameConfig.config.GetRandomStarType();
                _boardDatas[i,j] = starType;

                string starName = gameConfig.config.GetStarName(starType);

                GameEntity entity = CreateEntity();

                Debug.Log(string.Format("Name:{0} Type:{1} Row:{2} Col:{3}",starName,starType,i,j));
            }
            Debug.Log("");
        }
    }

}
