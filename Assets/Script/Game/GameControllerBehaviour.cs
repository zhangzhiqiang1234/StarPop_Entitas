using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFrameWork;

public class GameControllerBehaviour : MonoBehaviour
{
    public ScriptableGameConfig gameConfig;
    private GameController _gameController;
    void Awake()
    {
        DrivesEntry drives = new DrivesEntry(
            new UnityTimeDrive(),
            new UnityLogDrive(),
            //new FileLogDrive(),
            new UnityInputDrive());

        _gameController = new GameController(gameConfig, drives);
    }

    // Start is called before the first frame update
    void Start()
    {
        _gameController.Initialize();
        UIManager.Instance.ShowView(UIType.Fight_MainView);
    }

    // Update is called once per frame
    void Update()
    {
        _gameController.Update();
    }
}
