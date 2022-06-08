using UnityEngine;
using System;

public class LineCrossingDetector : AgentBehaviour // Agent or Mono ?
{

    private const float LINE_POS_X = 2.7f; // TODO: set the right value
    private const float THRESHOLD = 0.2f; // TODO: set the right value (to be tested with the robots)
    public IOManager ioManager; // TODO: instanciate this somewhere    
    private float prevX;
    private float currX;
    void Start()
    {   
        currX = gameObject.GetComponent<Rigidbody>().transform.localPosition.x;
    }

    void Update()
    {
        float posX = gameObject.GetComponent<Rigidbody>().transform.localPosition.x;
        prevX = currX;
        currX = posX;

        if (prevX <= LINE_POS_X && currX >= LINE_POS_X){
            ioManager.OnStoneThrown(gameObject.GetComponent<SimulationStone>());
        }

        if (prevX >= LINE_POS_X && currX <= LINE_POS_X){
            gameObject.GetComponent<SimulationStone>().ResetStone();
        }
    }


    private bool IsWithin(float posX)
    {
        return Math.Abs(posX - LINE_POS_X) < THRESHOLD; 
    }
}