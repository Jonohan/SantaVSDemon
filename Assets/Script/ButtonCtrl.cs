using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevelNumber(int levelNumber)
    {
        HandleScene.LoadLevelNumber(levelNumber);
    }

    public void LoadHomeMenu()
    {
        HandleScene.LoadHome();
    }

    public void PauseGame()
    {
        HandleScene.PauseGame();
    }

    public void ResumeGame()
    {
        HandleScene.ResumeGame();
    }
}
