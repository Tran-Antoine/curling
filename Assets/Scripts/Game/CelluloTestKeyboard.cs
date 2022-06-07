using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

//Input Keys
public enum InputKeyboard{
    arrows = 0, 
    ijkl = 1,
    mouse = 2
}

public class CelluloTestKeyboard : AgentBehaviour
{

    public InputKeyboard inputKeyboard; 
    public int number;

    // Start is called before the first frame update
    public void Start(){
        gameObject.tag = "Player";
        switch (number)
        {
            case 1 : 
                inputKeyboard = InputKeyboard.arrows;
                break;
            case 2 :
                inputKeyboard = InputKeyboard.ijkl;
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
                zmvt = -Input.GetAxis("HorizontalArrow");
                xmvt = Input.GetAxis("VerticalArrow");
                break;

            case InputKeyboard.ijkl : 
                zmvt = -Input.GetAxis("HorizontalIJKL");
                xmvt = Input.GetAxis("VerticalIJKL");
                break;
                
            case InputKeyboard.mouse :
                break;
        }

        Steering steering = new Steering();
        steering.linear = new Vector3(xmvt, 0, zmvt)* agent.maxAccel;
        steering.linear = this.transform.parent.TransformDirection(Vector3.ClampMagnitude(steering.linear , agent.maxAccel));
        return steering;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
