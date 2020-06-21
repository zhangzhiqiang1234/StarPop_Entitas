using System.Collections.Generic;
using Entitas;

public class ResultJudgeSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    public ResultJudgeSystem(Contexts contexts) : base(contexts.game)
    {
        _contexts = contexts;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            int rowNum = _contexts.game.levelInfo.boardRow;
            int colNum = _contexts.game.levelInfo.boardCol;

            for (int col = 0; col < colNum; col++)
            {
                for (int row = 0; row < rowNum; row++)
                {
                    GameEntity entity = _contexts.game.getEntityByRowAndCol(row, col);
                    //如果某一列的第一行为空的话，那么证明右面的的元素都消除了
                    if (entity == null )
                    {
                        if (row == 0)
                        {
                            //没有可以消除的星星了

                            return;
                        }
                        break;
                    }
                    //判断上下左右是否有相同的
                    //右
                    GameEntity entityR = _contexts.game.getEntityByRowAndCol(row, col+1);
                    if (entityR != null && entityR.star.starType == entity.star.starType)
                    {
                        return;
                    }
                    //右
                    GameEntity entityU = _contexts.game.getEntityByRowAndCol(row+1, col);
                    if (entityU != null && entityU.star.starType == entity.star.starType)
                    {
                        return;
                    }
                }
            }
            break;
        }

        //没有消除的了，延时1S，进入结算阶段
        bool a = _contexts.game.hasLevelInfo;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isCheckEnd;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.CheckEnd);
    }
}
