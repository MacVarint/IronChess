using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimerScript : MonoBehaviour
{
    public TextMeshPro fullTime;
    private string millisecondsText;
    private string secondsText;
    private string minutesText;
    public float timer = 0f;
    public int milliSeconds = 0;
    public int seconds = 0;
    public int minutes = 0;
    public bool turnOnTimer = true;
    public bool countDown = false;

    // Start is called before the first frame update
    void Start()
    {
        fullTime.text = "00:00:000";
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOnTimer)
        {
            Converter();
        }
    }
    public void Converter()
    {
        if (countDown)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime;
        }
        milliSeconds = (int)Mathf.Floor((timer % 1) * 1000);
        seconds = (int)Mathf.Floor(timer % 60);
        minutes = (int)Mathf.Floor(timer / 60);

        millisecondsText = milliSeconds.ToString("000");
        secondsText = seconds.ToString("00");
        minutesText = minutes.ToString("00");

        fullTime.text = "" + minutesText + ":" + secondsText + ":" + millisecondsText;
    }
}