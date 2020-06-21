using System;
using System.Collections.Generic;
using Entitas;

public class GainScoreSystem : ReactiveSystem<GameEntity>
{
    Contexts _contexts;
    public GainScoreSystem(Contexts contexts) : base(contexts.game)
    {

        _contexts = contexts;
    }

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
        {
            int starNum = e.gainScore.starNum;
            int score = 0;

            int step = 3;
            int offScore = 50;
            int baseScore = 100;

            while(starNum > 0)
            {
                int num = starNum > step ? step : starNum;
                starNum = starNum - step;
                score = score + num * baseScore;
                baseScore = baseScore + offScore;
            }

            LevelInfoComponent levelInfo = _contexts.game.levelInfo;
            _contexts.game.ReplaceLevelInfo(levelInfo.curLevelId, levelInfo.boardRow, levelInfo.boardCol, levelInfo.curScore + score, levelInfo.targetScore);

            //更新UI

            //判断游戏是否结束


        }
    }

    protected override bool Filter(GameEntity entity)
    {
        return entity.hasGainScore;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        return context.CreateCollector<GameEntity>(GameMatcher.GainScore);
    }
}