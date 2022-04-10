using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostSheepTimer : MonoBehaviour
{
    private float time;

    public Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        time = 0f    ;        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if(time < 0){
            Debug.Log("Time has run out");
        }

        displayTime(time);

    }

    void displayTime(float time){

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
