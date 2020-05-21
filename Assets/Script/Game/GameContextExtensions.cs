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

                string key = getIndexKey(i,j);
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

    public void ClearData(int row,int col)
    {
        if (isCorrectRow(row) && isCorrectCol(col))
        {
            _boardDatas[row, col] = -1;
            _boardEntityDic[getIndexKey(row, col)] = null;
        }
    }

    /// <summary>
    /// 广度优先遍历
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public List<GameEntity> DepthSearch(int row,int col)
    {
        List<GameEntity> resultList = new List<GameEntity>();
        Queue<GameEntity> queue = new Queue<GameEntity>();

        GameEntity curEntity = getEntityByRowAndCol(row, col);
        if (curEntity != null)
        {
            queue.Enqueue(curEntity);
            resultList.Add(curEntity);
            while (queue.Count > 0)
            {
                GameEntity popEntity = queue.Dequeue();
                int checkRow = popEntity.star.rowNum;
                int checkCol = popEntity.star.colNum;

                //左右
                for (int i = -1; i < 2; i = i + 2)
                {
                    int c = checkCol + i;
                    GameEntity checkEntity = getEntityByRowAndCol(checkRow, c);
                    if (checkEntity != null && checkEntity.star.starType == popEntity.star.starType && !resultList.Contains(checkEntity))
                    {
                        resultList.Add(checkEntity);
                        queue.Enqueue(checkEntity);
                    }
                }

                //上下
                for (int i = -1; i < 2; i = i + 2)
                {
                    int r = checkRow + i;
                    GameEntity checkEntity = getEntityByRowAndCol(r, checkCol);
                    if (checkEntity != null && checkEntity.star.starType == popEntity.star.starType && !resultList.Contains(checkEntity))
                    {
                        resultList.Add(checkEntity);
                        queue.Enqueue(checkEntity);
                    }
                }
            }
        }
        return resultList;
    }

    public GameEntity getEntityByRowAndCol(int row,int col)
    {
        if (isCorrectRow(row) && isCorrectCol(col))
        {
            return _boardEntityDic[getIndexKey(row, col)];
        }
        return null;
    }

    private bool isCorrectRow(int row)
    {
        return (row >= 0 && row < gameConfig.config.GetBoardRow());
    }

    private bool isCorrectCol(int col)
    {
        return (col >= 0 && col < gameConfig.config.GetBoardCol());
    }

    private string getIndexKey(int row,int col)
    {
        return string.Format("{0}_{1}", row, col);
    }
}
