using System;

[Serializable]
public class UIConfigDatas
{
    public UIConfigData[] Datas;
}

[Serializable]
public class UIConfigData
{
    public int uiType;
    public string prefabPath;
    public int uiLayer;
}

