using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI stage_text;
    public TextMeshProUGUI stagecounter;
    public void SetActive()
    {
        gameObject.SetActive(true);
        stage_text.SetText("YOU MADE IT TO STAGE " + stagecounter.GetParsedText());
    }

    public void RemoveActive()
    {
        gameObject.SetActive(false);
    }

    public void RestartButton()
    {        
        SceneManager.LoadSceneAsync("Game");
    }    
    public void MainMenuButton()
    {
        SceneManager.LoadSceneAsync("Game Menu");
    }
}
