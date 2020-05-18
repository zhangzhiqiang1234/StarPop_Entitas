using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameConfig
{
    string GetStarName(int starType);
    int GetRandomStarType();
    int GetBoardRow();
    int GetBoardCol();
}

[CreateAssetMenu(menuName = "StarPop/Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [SerializeField]
    private List<string> StarNames = new List<string>();
    [Range(4,18)]
    public int BoardRow;
    [Range(4,10)]
    public int BoradCol;
    public string GetStarName(int starType)
    {
        return StarNames[starType - 1];
    }

    public int GetRandomStarType()
    {
        return Random.Range(1, StarNames.Count);
    }

    public int GetBoardRow()
    {
        return this.BoardRow;
    }

    public int GetBoardCol()
    {
        return this.BoradCol;
    }
}