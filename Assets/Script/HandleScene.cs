using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class HandleScene
{
    private static int levelCount = 5;
    //private static int firstLevel = 2;
    private static bool isPaused = false;

    private static float startTime;
    private static float timePeriod = 0f;

    public static int redWins;
    public static int blueWins;


    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        HandleScene.ResumeGame();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }

    public static bool GetPauseStatus()
    {
        return isPaused;
    }

    public static void LoadNextLevel()
    {
        // stop timer and event collection cannot be put into if sentence like load prev level
        // because when the final level is finished, it also need to call this function for stop timer
        //StopTimer();
        if (LevelNumber() < levelCount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        }

        //StartTimer();

    }

    public static void LoadPrevLevel()
    {

        if (LevelNumber() > 1)
        {
            //StopTimer();

            // Debug.Log("load prev level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            //StartTimer();
        }

    }

    public static void LoadHome()
    {
        SceneManager.LoadScene(0);
    }

    public static bool isMaxLevel()
    {
        return LevelNumber() == levelCount;
    }

    public static bool isFirstLevel()
    {
        return LevelNumber() == 1;
    }

    public static int LevelNumber()
    {
        return SceneManager.GetActiveScene().buildIndex + 1;
    }

    public static void LoadLevelNumber(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public static GameObject FindSiblingGameObject(string name)
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            if (obj.name == name)
            {
                return obj;
            }
        }

        return null;
    }

    public static void StartTimer()
    {

        startTime = Time.time;
        timePeriod = 0f;
    }

    public static void StopTimer()
    {
        timePeriod = Time.time - startTime;
        timePeriod = (float)Math.Round(timePeriod / 60.0, 1);
        //Debug.Log("level play time:" + timePeriod);

    }


}
