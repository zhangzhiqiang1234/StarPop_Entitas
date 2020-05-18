using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerBehaviour : MonoBehaviour
{
    public ScriptableGameConfig gameConfig;
    private GameController _gameController;
    void Awake()
    {
        _gameController = new GameController(gameConfig);
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameController.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        _gameController.Update();
    }
}
