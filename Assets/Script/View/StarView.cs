using System.Collections;
using System.Collections.Generic;
using Entitas;
using Entitas.Unity;
using UnityEngine;

public class StarView : MonoBehaviour, IView, IPositionListener, IDestroyListener
{
    GameEntity gameEntity;

    public void Link(IEntity entity)
    {
        gameEntity = entity as GameEntity;
        gameObject.Link(gameEntity);

        gameEntity.AddPositionListener(this);
        gameEntity.AddDestroyListener(this);
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
        Debug.Log(string.Format("Change Position {0},{1} ", x, y));
    }
}
