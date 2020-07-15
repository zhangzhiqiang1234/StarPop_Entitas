

using System;

[Serializable]
public class LevelDatas
{
    public LevelData[] Datas;
}

[Serializable]
public class LevelData
{
    public int LevelId;
    public int TargetScore;
    public int EndTotalScore;
    public int EndPreScore;
    public int Row;
    public int Col;

    public int GetLevelId()
    {
        return LevelId;
    }

    public int GetTargetScore()
    {
        return TargetScore;
    }

    public int GetRow()
    {
        return Row;
    }

    public int GetCol()
    {
        return Col;
    }

    public int GetEndTotalScore()
    {
        return EndTotalScore;
    }

    public int GetEndPreScore()
    {
        return EndPreScore;
    }

}
