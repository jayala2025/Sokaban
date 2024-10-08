using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    Button startGameButton;
    void Start()
    {
        startGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
        startGameButton.onClick.AddListener(PlayGame);
    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level1");
    }
}
