using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInit : GameState
{
    [SerializeField] private GameObject mainMenuPanel;

    public override void Construct()
    {
        mainMenuPanel.SetActive(true);
    }
    public override void Destruct()
    {
        mainMenuPanel.SetActive(false);
    }
    public override void UpdateState()
    {
        /*if (InputManager.Instance.Tap)
        {
            gameManager.ChangeState(GetComponent<GameStateGame>());
        }*/
    }
}
