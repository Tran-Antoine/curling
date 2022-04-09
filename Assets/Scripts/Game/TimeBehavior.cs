using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TimeBehavior : MonoBehaviour
{   
    private const int TIE = -999;
    public TextMeshProUGUI ghostText;
    public TextMeshProUGUI globalText;
    public TextMeshProUGUI gameOverText;
    public Button restart;
    public Button back;
    private GameObject[] players;
    public Slider slider;
    public Button startButton;

    public const float GAME_DURATION = 120.0f;
    //the delay between each recomputation for the ghost mode
    private const float RECOMPUTE_TIME = 15.0f;

    private System.Random rand = new System.Random();

    private float time = RECOMPUTE_TIME;
    private bool isOnGhost = false;
    private float timer;

    //the global duration of a game
    private float globalTimer = 0.0f;

    private int minute = 0;
    private int seconde = 0;

    public AudioClip ghostMode;
    public AudioClip sheepMode;
    public AudioSource modeSwitch;

    private GameObject sheep;


    // Start is called before the first frame update
    void Start()
    {   

        ghostText.enabled = false;
        globalText.enabled = true;
        gameOverText.enabled = false;
        back.gameObject.SetActive(true);
        restart.gameObject.SetActive(false);
        startButton.gameObject.SetActive(true);
        slider.value = 0;
        Time.timeScale = 0f;
        modeSwitch = GetComponent<AudioSource>();
        

        if (players == null){
            players = GameObject.FindGameObjectsWithTag("Player");
        }   
        sheep = GameObject.FindWithTag("Sheep");
    }

    //generates a random time for the ghost mode
    float GhostTime(){
        float time = (float) rand.Next(12, 24);
        int choice = rand.Next(0,50);
        return choice <= 30 ? time : 0;
    }

    (int, int) getWinner(){
        int maxScore = TIE;
        int playerNumber = 0;
        for (int i = 0; i < players.Length; ++i) {
            int score = players[i].GetComponent<MoveWithKeyboardBehavior>().score;
            if (score == maxScore) return (-1, maxScore);

            if (score > maxScore){
                maxScore = score;
                playerNumber = players[i].GetComponent<MoveWithKeyboardBehavior>().number;
            }
        }

        return (playerNumber, maxScore);
    }
    
    private Color p1 = new Color(66/256f, 135/256f, 60/256f);
    private Color p2 = new Color(204/256f, 155/256f, 45/256f);
    private Color sheepColor = new Color(33/256f, 192/256f, 212/256f);
    private Color ghostColor = new Color(256/256f, 45/256f, 45/256f);


    // Update is called once per frame
    void Update(){ 
        if (startButton.IsActive()) {
            //Time.timeScale = 0f;
            return; 
        }
        
        Time.timeScale = 1f;
        globalTimer += Time.deltaTime;
        
        if (globalTimer > GAME_DURATION + 1) {
            Time.timeScale = 0f;
            (int player, int score) text = getWinner();
            gameOverText.gameObject.SetActive(true);
            gameOverText.enabled = true;
            
            if (text.player == -1){
                gameOverText.color = new Color(66f/256f, 135f/256f, 60f/256f);
                gameOverText.text = "It's a tie ! Both players win with " + text.score.ToString() + " points";
            } else {
            gameOverText.color = text.player == 1 ? p1 : p2;
            gameOverText.text = "Player " + text.player.ToString("0") + " won! Score : " + text.score.ToString();
            }

            restart.gameObject.SetActive(true);
            back.gameObject.SetActive(false);

        } else {
        slider.value = globalTimer / GAME_DURATION;
        minute = (int) Math.Floor(Convert.ToDouble(globalTimer)/ 60.0);
        seconde = (int) (globalTimer % 60);
        globalText.text = "Time  " + minute.ToString("00")  + ":" + seconde.ToString("00");

        time -= Time.deltaTime;

        if (time <= 0 && !isOnGhost){
            timer = GhostTime();
            if(timer > 0){
                sheep.tag = "Ghost";
                sheep.GetComponent<CelluloAgent>().SetVisualEffect(VisualEffect.VisualEffectConstAll, ghostColor, 255);
                modeSwitch.PlayOneShot(ghostMode);
                isOnGhost = true;
                ghostText.gameObject.SetActive(true);
                ghostText.enabled = true;
            } 

        } 

        if(isOnGhost){
            timer -= Time.deltaTime;

            if (timer <= 0){
                sheep.tag = "Sheep";
                sheep.GetComponent<CelluloAgent>().SetVisualEffect(VisualEffect.VisualEffectConstAll, sheepColor, 255);
                modeSwitch.PlayOneShot(sheepMode);
                isOnGhost = false; 
                time = RECOMPUTE_TIME;
                ghostText.gameObject.SetActive(false);
            }
            ghostText.text = "Ghost : " + Math.Round(timer, 0).ToString("00");
        }
      }
    }
}
