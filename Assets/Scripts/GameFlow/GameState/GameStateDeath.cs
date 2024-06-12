using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateDeath : GameState
{
    [SerializeField] private GameObject pausePanel;
    public override void Construct()
    {
        pausePanel.SetActive(true);
    }
    public override void Destruct()
    {
        pausePanel.SetActive(false);
    }
}
