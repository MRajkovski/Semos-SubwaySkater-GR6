using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    private GameState state;
    public PlayerMotor motor;

    private void Awake()
    {
        instance = this;
        state = GetComponent<GameStateInit>();
        state.Construct();
    }
    private void Update()
    {
        state.UpdateState();
    }
    public void ChangeState(GameState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void StartGame()
    {
        ChangeState(GetComponent<GameStateGame>());
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
