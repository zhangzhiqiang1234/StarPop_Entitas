using System.Collections.Generic;
using UnityEngine;

public partial class GameContext
{
    private int[,] _boardDatas;
    private Dictionary<string,GameEntity> _boardEntityDic = new Dictionary<string,GameEntity>();
    private float startPosX;
    private float startPosY;
    public EventDispatcher eventDispatcher = new EventDispatcher();

    public void InitBoradDatas(int row,int col)
    {
        _boardDatas = new int[row, col];
        _boardEntityDic.Clear();
        startPosX = (float)(-(col - 1) / 2.0);
        startPosY = (float)(-(row - 1) / 2.0);
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                int starType = gameConfig.config.GetRandomStarType();
                _boardDatas[i,j] = starType;

                string starName = gameConfig.config.GetStarName(starType);

                GameEntity entity = CreateEntity();
                entity.AddStar(starType,i,j);

                entity.AddAsset(gameConfig.config.GetStarName(starType));

                entity.AddPosition(startPosX+(j),startPosY+(i));

                string key = string.Format("{0}_{1}", i, j);
                if(_boardEntityDic.ContainsKey(key))
                {
                    _boardEntityDic[key] = entity;
                }
                else
                {
                    _boardEntityDic.Add(key,entity);
                }
            }
        }
    }
    public float GetStartPosX()
    {
        return startPosX;
    }

    public float GetStartPosY()
    {
        return startPosY;
    }
}
