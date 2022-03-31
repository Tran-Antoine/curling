using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Input Keys
public enum InputKeyboard{
    arrows =0, 
    wasd = 1
}

public class MoveWithKeyboardBehavior : AgentBehaviour
{
    public InputKeyboard inputKeyboard; 

    public void Start(){
        gameObject.tag = "Player";
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

}
