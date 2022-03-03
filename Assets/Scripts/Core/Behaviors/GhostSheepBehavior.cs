using System.Linq;
using UnityEngine;

public class SheepBehavior : AgentBehaviour
{    
    public void Start(){
    }
    public override Steering GetSteering()
    {
        
        Steering steering = new Steering();
        //implement your code here.
        return steering;
    }



}
