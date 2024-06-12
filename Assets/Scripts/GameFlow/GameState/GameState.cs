using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameState : MonoBehaviour
{
    protected GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }
    public virtual void Construct() { }
    public virtual void Destruct() { }
    public virtual void UpdateState() { }
}
