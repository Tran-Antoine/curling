using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

//Input Keys
public enum InputKeyboard{
    arrows =0, 
    wasd = 1
}

public class MoveWithKeyboardBehavior : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public int number;
    public AudioSource lost;

    public void Start(){
        gameObject.tag = "Player";
        lost = GetComponent<AudioSource>();
    }

    public override Steering GetSteering()
       {   
        float xmvt = (inputKeyboard == InputKeyboard.arrows) ? Input.GetAxis("HorizontalArrow") : Input.GetAxis("HorizontalWASD");
        float zmvt = (inputKeyboard == InputKeyboard.arrows) ? Input.GetAxis("VerticalArrow") : Input.GetAxis("VerticalWASD");
        Steering steering = new Steering();
        steering.linear = new Vector3(xmvt, 0, zmvt)* agent.maxAccel;
        steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }

    void  OnCollisionEnter(Collision collisionInfo){
        //Debug.Log(other.transform.parent.gameObject.name + " triggers.");
        //Debug.Log("Tag of collider : " + other.attachedRigidbody.GetComponent<CelluloAgent>().tag);
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
