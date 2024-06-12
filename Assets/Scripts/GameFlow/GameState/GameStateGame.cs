using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateGame : GameState
{
    public override void Construct()
    {
        GameManager.Instance.motor.ResumePlayer();
    }
}
