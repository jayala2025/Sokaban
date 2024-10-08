using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    Button endGameButton;
    void Start()
    {
        endGameButton = GameObject.Find("EndGameButton").GetComponent<Button>();
        endGameButton.onClick.AddListener(PlayGame);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level1");
    }
}
