﻿using System.Collections.Generic;
using UnityEngine;

public partial class GameContext
{
    private int[,] _boardDatas;
    private Dictionary<string,GameEntity> _boardEntityDic = new Dictionary<string,GameEntity>();
    private float startPosX;
    private float startPosY;
    private int currentStarNum;

    public void InitBoradDatas(int row,int col)
    {
        _boardDatas = new int[row, col];
        _boardEntityDic.Clear();
        currentStarNum = row * col;
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
            currentStarNum--;
        }
    }

    public int getStarType(int row, int col)
    {
        if (isCorrectRow(row) && isCorrectCol(col))
        {
            return _boardDatas[row, col];
        }
        return -1;
    }

    public GameEntity getStarEntity(int row, int col)
    {
        if (isCorrectRow(row) && isCorrectCol(col))
        {
            return _boardEntityDic[getIndexKey(row, col)];
        }
        return null;
    }

    public void swapStar(int row1,int col1,int row2,int col2)
    {
        if (isCorrectRow(row1) && isCorrectCol(col1)&&isCorrectRow(row2) && isCorrectCol(col2))
        {
            int temp = _boardDatas[row1, col1];
            _boardDatas[row1, col1] = _boardDatas[row2, col2];
            _boardDatas[row2, col2] = temp;

            GameEntity tempEntity = _boardEntityDic[getIndexKey(row1, col1)];
            _boardEntityDic[getIndexKey(row1, col1)] = _boardEntityDic[getIndexKey(row2, col2)];
            _boardEntityDic[getIndexKey(row2, col2)] = tempEntity;
        }
    }

    /// <summary>
    /// 广度优先遍历
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public List<GameEntity> BFSearch(int row,int col)
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

    /// <summary>
    /// 深度优先遍历
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public List<GameEntity> DFSearch(int row,int col)
    {
        List<GameEntity> resultList = new List<GameEntity>();
        Stack<GameEntity> stack = new Stack<GameEntity>();

        GameEntity curEntity = getEntityByRowAndCol(row, col);
        if (curEntity != null)
        {
            stack.Push(curEntity);
            while (stack.Count > 0)
            {
                GameEntity popEntity = stack.Pop();
                resultList.Add(popEntity);
                int checkRow = popEntity.star.rowNum;
                int checkCol = popEntity.star.colNum;

                //左右
                for (int i = -1; i < 2; i = i + 2)
                {
                    int c = checkCol + i;
                    GameEntity checkEntity = getEntityByRowAndCol(checkRow, c);
                    if (checkEntity != null && checkEntity.star.starType == popEntity.star.starType && !resultList.Contains(checkEntity))
                    {
                        if (!stack.Contains(checkEntity))
                        {
                            stack.Push(checkEntity);
                        }
                    }
                }

                //上下
                for (int i = -1; i < 2; i = i + 2)
                {
                    int r = checkRow + i;
                    GameEntity checkEntity = getEntityByRowAndCol(r, checkCol);
                    if (checkEntity != null && checkEntity.star.starType == popEntity.star.starType && !resultList.Contains(checkEntity))
                    {
                        if (!stack.Contains(checkEntity))
                        {
                            stack.Push(checkEntity);
                        }
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
        return (row >= 0 && row < levelInfo.boardRow);
    }

    private bool isCorrectCol(int col)
    {
        return (col >= 0 && col < levelInfo.boardCol);
    }

    public string getIndexKey(int row,int col)
    {
        return string.Format("{0}_{1}", row, col);
    }

    public int getStarNum()
    {
        return currentStarNum;
    }

    public void getRangRowAndCol(List<GameEntity> entities,out int minRow,out int maxRow,out int minCol,out int maxCol)
    {

        entities.Sort((e1, e2) =>
        {
            if (e1.star.rowNum > e2.star.rowNum)
                return 1;
            else if (e1.star.rowNum == e2.star.rowNum)
                return 0;
            else
                return -1;
        });
        minRow = entities[0].star.rowNum;
        maxRow = entities[entities.Count - 1].star.rowNum;

        entities.Sort((e1, e2) =>
        {
            if (e1.star.colNum > e2.star.colNum)
                return 1;
            else if (e1.star.colNum == e2.star.colNum)
                return 0;
            else
                return -1;
        });
        minCol = entities[0].star.colNum;
        maxCol = entities[entities.Count - 1].star.colNum;
    }
}
