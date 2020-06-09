using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameConfig
{
    string GetStarName(int starType);
    int GetRandomStarType();
    //int GetBoardRow();
    //int GetBoardCol();
    Color GetSelectColor(int starType);

    void LoadLevelData(string filename);
    LevelData GetLevelData(int levelId);

}

[CreateAssetMenu(menuName = "StarPop/Game Config")]
public class ScriptableGameConfig : ScriptableObject, IGameConfig
{
    [SerializeField]
    private List<string> StarNames = new List<string>();
    [SerializeField]
    private List<Color> StarSelectColors = new List<Color>();
    private Dictionary<int,LevelData> Dic_LevelDatas = new Dictionary<int, LevelData>();
    //[Range(4,18)]
    //public int BoardRow;
    //[Range(4,10)]
    //public int BoradCol;
    public string GetStarName(int starType)
    {
        return StarNames[starType - 1];
    }

    public int GetRandomStarType()
    {
        return Random.Range(1, StarNames.Count+1);
    }

    //public int GetBoardRow()
    //{
    //    return this.BoardRow;
    //}

    //public int GetBoardCol()
    //{
    //    return this.BoradCol;
    //}

    public Color GetSelectColor(int starType)
    {
        return StarSelectColors[starType - 1];
    }

    public void LoadLevelData(string filename)
    {
        string datastr = Resources.Load<TextAsset>(filename).text;
        LevelDatas datas = JsonUtility.FromJson<LevelDatas>(datastr);
        for (int i = 0; i < datas.Datas.Length; i++)
        {
            LevelData data = datas.Datas[i];
            Dic_LevelDatas.Add(data.GetLevelId(), data);
        }     
    }

    public LevelData GetLevelData(int levelId)
    {
       return Dic_LevelDatas[levelId];
    }
}