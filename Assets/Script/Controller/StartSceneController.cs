using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    public Button startBtn;
    public void Start()
    {
        init();
    }

    public void init()
    {
        startBtn.onClick.AddListener(startGameClick);
    }

    public void startGameClick()
    {
        GameManager.gameManager.startGame(closeUI);
    }

    public void closeUI()
    {

    }
}
