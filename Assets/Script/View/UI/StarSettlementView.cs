using UIFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class StarSettlementView : BaseView
{
    private Text txtScore;

    // Start is called before the first frame update
    protected override void OnActive()
    {
        txtScore = transform.Find("txtRewardScore").GetComponent<Text>();
    }

    public void UpdateScore(float score)
    {
        txtScore.text = score.ToString();
    }
}
