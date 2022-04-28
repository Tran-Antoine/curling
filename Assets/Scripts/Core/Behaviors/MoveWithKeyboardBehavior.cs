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
    //private GameObject player;

    public void Start(){
        gameObject.tag = "Player";
        lost = GetComponent<AudioSource>();
        //player =  GameObject.FindGameObjectWithTag("Player").transform.position;

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
        
        /**float xmvt = 0f;
        float zmvt =*/
        //float zmvt; 
        
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
                Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 cellulo = gameObject.transform.position;
                Vector3 dir = pz - cellulo;
                xmvt = dir.x;
                zmvt = dir.z;
                break;
        }

        Steering steering = new Steering();
        steering.linear = new Vector3(xmvt, 0, zmvt)* agent.maxAccel;
        steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }

    void  OnCollisionEnter(Collision collisionInfo){
        if(collisionInfo.gameObject.CompareTag("Ghost")){
            updateScore(score-1);
            lost.Play();
        }
    }

    public void updateScore(int i){
        score = i;
        scoreText.text = score.ToString();
    }
}
