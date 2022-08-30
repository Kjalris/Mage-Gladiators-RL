using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text stage_text;
    public void SetActive(int stage)
    {
        gameObject.SetActive(true);
        stage_text.SetText("YOU MADE IT TO STAGE " + stage.ToString());
    }

    public void RemoveActive()
    {
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }    
    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
