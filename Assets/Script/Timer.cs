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
    public GameObject player1;
    public GameObject player2;

    private float prevTime;

    // Start is called before the first frame update
    void Start()
    {
        timeRemain = 180f;
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
                timeText.GetComponentInChildren<TMP_Text>().text = "0"+minutes.ToString()+":"+seconds.ToString();
            }
        } else
        {
            
        }
        prevTime = curTime;
    }
}
