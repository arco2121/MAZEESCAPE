using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class timescript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    // float elapsedTime;
    [SerializeField] float remainingtime;


    void Update()
    {
        if (remainingtime > 0)
        {
            remainingtime -= Time.deltaTime;
        }
        else if (remainingtime < 0)
        {
            remainingtime = 0;
            SceneManager.LoadScene("Lose");
        }
        //elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(remainingtime / 60);
        int seconds = Mathf.FloorToInt(remainingtime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);  // Aggiunta parentesi mancante
    }

    public void LoseTime(float seconds)
    {
        remainingtime -= seconds;
    }

    public void GainTime(float seconds)
    {
        remainingtime += seconds;
    }
}
