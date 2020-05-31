using System.Collections.Generic;
using Entitas;

public class SelectStarSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    public SelectStarSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        if (entities.Count > 0)
        {
            GameEntity clickEntity = entities[0];
            GameEntity selectEntity = _contexts.game.getEntityByRowAndCol(clickEntity.clickStar.row, clickEntity.clickStar.col);

            if (selectEntity != null)
            {
                if (selectEntity.hasSelectStar && selectEntity.selectStar.isSelect)
                {
                    //删除被选中的星星
                    List<GameEntity> resultList = _contexts.game.DFSearch(clickEntity.clickStar.row, clickEntity.clickStar.col);
                    foreach (var item in resultList)
                    {
                        item.ReplaceSelectStar(false,false,false,false,false);
                    }

                }
                else
                {
                    List<GameEntity> resultList = _contexts.game.DFSearch(clickEntity.clickStar.row, clickEntity.clickStar.col);
                    if (resultList.Count > 1)
                    {
                        resultList.Sort((e1,e2)=> 
                        {
                            if (e1.star.rowNum > e2.star.rowNum)
                                return 1;
                            else if (e1.star.rowNum == e2.star.rowNum)
                                return 0;
                            else
                                return -1;
                        });
                        int minRow = resultList[0].star.rowNum;
                        int maxRow = resultList[resultList.Count - 1].star.rowNum;

                        resultList.Sort((e1, e2) =>
                        {
                            if (e1.star.colNum > e2.star.colNum)
                                return 1;
                            else if (e1.star.colNum == e2.star.colNum)
                                return 0;
                            else
                                return -1;
                        });
                        int minCol = resultList[0].star.colNum;
                        int maxCol = resultList[resultList.Count - 1].star.colNum;

                        Dictionary<string, GameEntity> dicEntity = new Dictionary<string, GameEntity>();
                        foreach (var item in resultList)
                        {
                            dicEntity.Add(_contexts.game.getIndexKey(item.star.rowNum,item.star.colNum),item);
                        }

                        for (int i = minRow; i <= maxRow; i++)
                        {
                            for (int j = minCol; j <= maxCol; j++)
                            {
                                string key = _contexts.game.getIndexKey(i, j);
                                GameEntity entity;
                                if (dicEntity.TryGetValue(key,out entity))
                                {
                                    bool select = true;
                                    bool up = false;
                                    bool down = false;
                                    bool left = false;
                                    bool right = false;
                                    GameEntity e;

                                    //上
                                    if (!dicEntity.TryGetValue(_contexts.game.getIndexKey(i + 1, j), out e))
                                    {
                                        up = true;
                                    }

                                    //下
                                    if (!dicEntity.TryGetValue(_contexts.game.getIndexKey(i - 1, j), out e))
                                    {
                                        down = true;
                                    }

                                    //左
                                    if (!dicEntity.TryGetValue(_contexts.game.getIndexKey(i, j - 1), out e))
                                    {
                                        left = true;
                                    }

                                    //右
                                    if (!dicEntity.TryGetValue(_contexts.game.getIndexKey(i, j + 1), out e))
                                    {
                                        right = true;
                                    }
                                    if (entity.hasSelectStar)
                                    {
                                        entity.ReplaceSelectStar(select, up, down, left, right);
                                    }
                                    else
                                    {
                                        entity.AddSelectStar(select, up, down, left, right);
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }
            clickEntity.isDestroy = true;
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasClickStar && !entity.isDestroy;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.ClickStar);
    }
}
