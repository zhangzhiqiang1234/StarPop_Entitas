using System;
using Entitas;
using UnityEngine;

public class SettlementSystem : IExecuteSystem
{
    private Contexts _contexts;
    private Group<GameEntity> _group;

    public SettlementSystem(Contexts contexts)
    {
        _contexts = contexts;
        _group = (Group<GameEntity>)_contexts.game.GetGroup(GameMatcher.Settlement);
    }
    public void Execute()
    {
        for (int i = 0; i < _group.GetEntities().Length; i++)
        {
            GameEntity e = _group.GetEntities()[i];
            if (e.settlement.dealyTime <= 0)
            {
                Settlement();
                e.isDestroy = true;
                return;
            }
            else
            {
                e.settlement.dealyTime -= _contexts.game.time.TimeDrive.GetDeltaTime();
            }
        }
    }

    private void Settlement()
    {
        LevelInfoComponent levelInfo = _contexts.game.levelInfo;
        int totalScore = levelInfo.endTotalScore;
        int preScore = levelInfo.endPreScore;
        int starNum = _contexts.game.getStarNum();
        int addScore = totalScore - (starNum * preScore) > 0 ? totalScore - (starNum * preScore) : 0;
        _contexts.game.ReplaceLevelInfo(levelInfo.curLevelId,levelInfo.boardRow, levelInfo.boardCol, levelInfo.curScore+addScore, levelInfo.endTotalScore,levelInfo.endPreScore, levelInfo.targetScore);

        //通知UI表现
        _contexts.game.eventDispatcher.dispatchEvent<int, int, int, int>(EventEnum.FIGHT_SETTLEMENT, starNum, totalScore, preScore, levelInfo.curScore);
    }
}
