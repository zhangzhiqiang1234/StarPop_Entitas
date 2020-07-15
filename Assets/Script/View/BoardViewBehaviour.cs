using Entitas.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardViewBehaviour : MonoBehaviour
{
    private GameContext _context;
    private void Awake()
    {
        _context = Contexts.sharedInstance.game;
        _context.eventDispatcher.addEventListener<GameEntity>(EventEnum.FIGHT_CREATEVIEW, this.addStarView);
        _context.eventDispatcher.addEventListener<int, int, int, int>(EventEnum.FIGHT_SETTLEMENT, this.settlementPerform);
    }

    private void settlementPerform(int starNum, int totalScore, int preScore, int curScore)
    {
        StartCoroutine(deleteStarPerform());
    }

    private IEnumerator deleteStarPerform()
    {
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            GameObject starObject = gameObject.transform.GetChild(i).gameObject;
            //starObject.Unlink();
            //GameObject.Destroy(starObject);
            starObject.SetActive(false);
            yield return new WaitForSeconds(1.0f);
        }

        GameEntity gameEntity = _context.CreateEntity();
        gameEntity.isChangeLevel = true;


    }

    private void clearStarView()
    {
        for (int i = gameObject.transform.childCount - 1;  i >= 0; i--)
        {
            GameObject starObject = gameObject.transform.GetChild(i).gameObject;
            starObject.Unlink();
            GameObject.Destroy(starObject);
        }
    }

    private void addStarView(GameEntity entity)
    {
        string name = entity.asset.assetName;
        float x = entity.position.x;
        float y = entity.position.y;
        var prefab = Resources.Load<GameObject>(name);
        GameObject starObject = GameObject.Instantiate(prefab,this.gameObject.transform);
        IView view = starObject.GetComponent<IView>();
        view.Link(entity);
        starObject.transform.localPosition = new Vector3(x, y, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        _context.eventDispatcher.removeEventListener<GameEntity>(EventEnum.FIGHT_CREATEVIEW, this.addStarView);
        _context.eventDispatcher.removeEventListener<int,int,int,int>(EventEnum.FIGHT_SETTLEMENT, this.settlementPerform);
    }
}
