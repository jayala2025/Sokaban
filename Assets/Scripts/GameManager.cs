using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;



public class GameManager : MonoBehaviour
{
    //public Button startButton;
    
    public int totalMovableBlocks = 0;

    private int blocksOnTargetTiles = 0;

    public bool isLevelCompleted = false;

    public void SetTotalMovableBlocks(int count)
    {
        totalMovableBlocks = count;
    }

    public void IncrementBlocksOnTargetTiles()
    {
        blocksOnTargetTiles++;
        CheckLevelCompletion();
    }

    public void DecrementBlocksOnTargetTiles()
    {
        blocksOnTargetTiles--;
    }

    private void CheckLevelCompletion()
    {
        if(blocksOnTargetTiles == totalMovableBlocks)
        {
            CompletedLevel();
        }
    }

    public void CompletedLevel()
    {
        isLevelCompleted = true;
        Debug.Log("Level Completed");

        Invoke("LoadNextLevel", 2f);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else 
        {

            Debug.Log("You have completed all levels!");
            SceneManager.LoadScene("EndGame");
        }
    }
}
