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

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        //implement your code here
        return steering;
    }

}
