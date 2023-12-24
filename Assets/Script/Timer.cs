using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeRemain;
    private float countdown;
    public GameObject timeText;
    public GameObject countdownText;
    public GameObject endGamePanel;
    public GameObject winPlayer;
    public GameObject score;
    public GameObject percent;
    public GameObject player1;
    public GameObject player2;
    public GameObject ground;

    private Color white = new Color(1f, 1f, 1f, 1f);
    private Color red = new Color(1f, 0.2f, 0.2f, 1f);
    private float prevTime;

    private int deltaTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeRemain = 10f; //Level time (sec)
        countdown = 2.9f;
        prevTime = Time.time;
        //InputSystem.DisableDevice(Keyboard.current);
        player1.GetComponent<PlayerController>().active = false;
        player2.GetComponent<PlayerController>().active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        float curTime = Time.time;
        //Debug.Log(curTime - prevTime);
        if (timeRemain > 0)
        {
            if (countdown > 0)
            {
                countdown -= curTime - prevTime;
                countdownText.GetComponentInChildren<TMP_Text>().text = ((int) countdown + 1).ToString();
                if (countdown <= 0)
                {
                    //InputSystem.EnableDevice(Keyboard.current);
                    player1.GetComponent<PlayerController>().active = true;
                    player2.GetComponent<PlayerController>().active = true;
                    countdownText.SetActive(false);
                }
                timeText.GetComponentInChildren<TMP_Text>().text = "03:00";
            } else {
                timeRemain -= curTime - prevTime;
                int minutes = (int)(timeRemain / 60f);
                int seconds = (int)(timeRemain - minutes*60f);
                if (seconds < 10) {
                    timeText.GetComponentInChildren<TMP_Text>().text = "0"+minutes.ToString()+":0"+seconds.ToString();
                } else {
                    timeText.GetComponentInChildren<TMP_Text>().text = "0"+minutes.ToString()+":"+seconds.ToString();
                }
            }
            if (timeRemain < 10) {
                deltaTime--;
                if (deltaTime <= 0) {
                    if (timeText.GetComponentInChildren<TMP_Text>().color == red) {
                        timeText.GetComponentInChildren<TMP_Text>().color = white;
                    } else {
                        timeText.GetComponentInChildren<TMP_Text>().color = red;
                    }
                     deltaTime = 20;
                }
                
            } 
            if (timeRemain <= 0) {
                timeText.GetComponentInChildren<TMP_Text>().text = "00:00";
                timeText.GetComponentInChildren<TMP_Text>().color = red;
                endGamePanel.SetActive(true);
                EndGame();
            }
        } 
        prevTime = curTime;
    }

    public void EndGame() {
        float redPerc = ground.GetComponent<ground>().redPerc();
        float bluePerc = ground.GetComponent<ground>().bluePerc();
        Debug.Log(redPerc);
        Debug.Log(bluePerc);
        if (redPerc > bluePerc) {
            winPlayer.GetComponentInChildren<TMP_Text>().text = "Demon";
            HandleScene.redWins++;
        } else {
            winPlayer.GetComponentInChildren<TMP_Text>().text = "Santa";
            HandleScene.blueWins++;
        }
        percent.GetComponentInChildren<TMP_Text>().text = percToString(bluePerc) + " vs " + percToString(redPerc);
        score.GetComponentInChildren<TMP_Text>().text = (HandleScene.blueWins.ToString() + " : "+ HandleScene.redWins.ToString());
    }

    private string percToString(float perc) {
        if (perc < 10) {
            return "0"+Mathf.Round(perc).ToString()+"%";
        } else {
            return Mathf.Round(perc).ToString()+"%";
        }
    }

}
