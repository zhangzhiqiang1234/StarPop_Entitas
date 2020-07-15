using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class StarView : MonoBehaviour, IView, IPositionListener, IDestroyListener, ISelectStarListener
{
    public void Link(IEntity entity)
    {
        GameEntity gameEntity = entity as GameEntity;
        gameObject.Link(gameEntity);

        gameEntity.AddPositionListener(this);
        gameEntity.AddDestroyListener(this);
        gameEntity.AddSelectStarListener(this);
    }

    private void Awake()
    {
        
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
        
    }

    public void OnDestroy(GameEntity entity)
    {
        gameObject.Unlink();
        Object.Destroy(this.gameObject);
    }

    public void OnPosition(GameEntity entity, float x, float y)
    {
        if (!entity.isDestroy)
        {
            this.transform.localPosition = new Vector3(x, y, this.transform.localPosition.z);
        }
    }

    public void OnSelectStar(GameEntity entity, bool isSelect, bool showUpEdge, bool showDownEdge, bool showLeftEdge, bool showRightEdge)
    {
        int starType = entity.star.starType;
        Color selectColor = Contexts.sharedInstance.game.gameConfig.config.GetSelectColor(starType);
        SetSelect(isSelect,showUpEdge,showDownEdge,showLeftEdge,showRightEdge,selectColor);
    }

    private void SetSelect(bool isSelect, bool showUpEdge, bool showDownEdge, bool showLeftEdge, bool showRightEdge,Color color)
    {
        Material material = gameObject.GetComponent<SpriteRenderer>().material;
        if (!isSelect)
        {
            showUpEdge = false;
            showDownEdge = false;
            showLeftEdge = false;
            showRightEdge = false;
        }
        material.SetFloat("_ShowUpEdge", showUpEdge ? 1 : 0);
        material.SetFloat("_ShowDownEdge", showDownEdge ? 1 : 0);
        material.SetFloat("_ShowLeftEdge", showLeftEdge ? 1 : 0);
        material.SetFloat("_ShowRightEdge", showRightEdge ? 1 : 0);
        material.SetColor("_EdgeColor", color);
    }
}
