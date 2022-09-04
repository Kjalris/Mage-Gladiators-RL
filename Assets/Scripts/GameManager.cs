using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool GameIsOver;

    public GameOverScreen game_over_screen;
    public GameObject stageCounter;
    public PlayerManagement playerManagement;

    void Start()
    {
        GameIsOver = false;
    }

    void Update()
    {
        if (GameIsOver)
            return;

        // End the game if the players hp reached 0 or lower
        if (playerManagement.GetCurrentHP() <= 0)
        {
            EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("Game Menu");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void EndGame()
    {
        game_over_screen.SetActive();
        stageCounter.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameIsOver = true;
    }
}
