using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameConfig
{
    string GetStarName(int starType);
    int GetRandomStarType();
}


[CreateAssetMenu(menuName = "StarPop/Game Config")]
public class ScriptableGameConfig : ScriptableObject,IGameConfig
{
    [SerializeField]
    public List<string> StarNames = new List<string>();
    public string GetStarName(int starType)
    {
        return StarNames[starType-1];
    }

    public int GetRandomStarType()
    {
        return Random.Range(1, StarNames.Count);
    }
}
