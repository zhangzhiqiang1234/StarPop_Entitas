using System.Collections.Generic;
using Entitas;

public class DestroyStarsSystem : ReactiveSystem<GameEntity>
{
    GameContext _gameContext;
    public DestroyStarsSystem(Contexts contexts) : base(contexts.game)
    {
        _gameContext = contexts.game;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        int minRow, maxRow, minCol, maxCol;
        _gameContext.getRangRowAndCol(entities, out minRow, out maxRow, out minCol, out maxCol);
        maxRow = _gameContext.levelInfo.boardRow;
        maxCol = _gameContext.levelInfo.boardCol;
        int downStep = 0;
        int leftStep = 0;
        bool isEmptyCol = false;

        for (int col = minCol; col < maxCol; col++)
        {
            isEmptyCol = minRow <= 0;
            downStep = 0;
            for (int row = minRow; row < maxRow; row++)
            {
                if (_gameContext.getStarType(row,col) != -1)
                {
                    isEmptyCol = false;

                    int newRow = row - downStep;
                    int newCol = col - leftStep;

                    _gameContext.swapStar(row, col, newRow, newCol);

                    GameEntity entity = _gameContext.getStarEntity(newRow,newCol);
                    if (entity != null)
                    {
                        entity.ReplaceStar(entity.star.starType,newRow,newCol);
                        entity.ReplacePosition(_gameContext.GetStartPosX() + newCol, _gameContext.GetStartPosY() + newRow);
                    }
                }
                else
                {
                    downStep++;
                }
            }
            if (isEmptyCol)
            {
                leftStep++;
            }
        }

         _gameContext.CreateEntity().AddGainScore(entities.Count);
        _gameContext.CreateEntity().isCheckEnd = true;
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.isDestroy && entity.hasStar && entity.hasSelectStar;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        int[] allof = {GameComponentsLookup.Destroy,GameComponentsLookup.Star,GameComponentsLookup.SelectStar };
        return context.CreateCollector<GameEntity>(GameMatcher.AllOf(allof));
    }
}
