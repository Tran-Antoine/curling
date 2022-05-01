using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

//Input Keys
public enum InputKeyboard{
    arrows = 0, 
    wasd = 1,
    mouse = 2
}

public class MoveWithKeyboardBehavior : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public int number;
    public AudioSource lost;

    private bool hasSpaceGem = false;
    private float timeLeftWithGem = 0f;
    private const float TOTAL_GEM_TIME = 8f;
    private SpaceGem spaceGem;
    public GameObject gemIndicator;
    public AudioSource pointsTheft;
    public AudioSource collected;
    public TimeBehavior timeBehavior;

    private bool clicked;

    public void Start(){
        gameObject.tag = "Player";
        AudioSource[] sounds = GetComponents<AudioSource>();
        lost = sounds[0];
        pointsTheft = sounds[1];
        collected = sounds[2];

        switch (number)
        {
            case 1 : 
                inputKeyboard = Settings.p1_keyboard;
                break;
            case 2 :
                inputKeyboard = Settings.p2_keyboard;
                break;
        }
    }


    float xmvt = 0f;
    float zmvt = 0f;

    public override Steering GetSteering()
       {  
        switch (inputKeyboard)
        {
            case InputKeyboard.arrows : 
                xmvt = Input.GetAxis("HorizontalArrow");
                zmvt = Input.GetAxis("VerticalArrow");
                break;

            case InputKeyboard.wasd : 
                xmvt = Input.GetAxis("HorizontalWASD");
                zmvt = Input.GetAxis("VerticalWASD");
                break;
                
            case InputKeyboard.mouse :
                if (clicked) {
                    Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 cellulo = gameObject.transform.position;
                    Vector3 dir = pz - cellulo;
                    xmvt = dir.x;
                    zmvt = dir.z;
                }
                break;
        }

        Steering steering = new Steering();
        steering.linear = new Vector3(xmvt, 0, zmvt)* agent.maxAccel;
        steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }

    void  OnCollisionEnter(Collision collisionInfo){
        if(collisionInfo.gameObject.CompareTag("Ghost")){
            updateScore(-1);
            lost.Play();
        }
        if(collisionInfo.gameObject.CompareTag("SpaceGem")){
            hasSpaceGem = true;
            timeLeftWithGem = TOTAL_GEM_TIME;
            spaceGem = collisionInfo.gameObject.GetComponent<SpaceGem>();
            spaceGem.caughtByPlayer();
            collected.Play();
            gemIndicator.SetActive(true);
        }
        if(collisionInfo.gameObject.CompareTag("Player") && hasSpaceGem){
            pointsTheft.Play();
            spaceGem.lostGem();
            collisionInfo.gameObject.GetComponent<MoveWithKeyboardBehavior>().updateScore(-2);
            updateScore(2);
            
            hasSpaceGem = false;
            timeLeftWithGem = 0f;
            gemIndicator.SetActive(false);
        }
        if(collisionInfo.gameObject.CompareTag("TimeGem")){
            collected.Play();
            timeBehavior.playerGotTimeGem(this);
            collisionInfo.gameObject.GetComponent<TimeGem>().caughtByPlayer();
        }
    }

    void Update(){
        if (timeLeftWithGem >= 0f){
            timeLeftWithGem -= Time.deltaTime;
        } else if(hasSpaceGem) {
            hasSpaceGem = false;
            spaceGem.lostGem();
            gemIndicator.SetActive(false);
        }
    }

    public void updateScore(int i){
        score += i;
        scoreText.text = score.ToString();
    }

    void OnMouseDown(){
        clicked = true;
    }
}
