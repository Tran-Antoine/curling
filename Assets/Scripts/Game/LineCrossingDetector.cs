using UnityEngine;
using System;

public class LineCrossingDetecter : AgentBehaviour // Agent or Mono ?
{

    private const float LINE_POS_X = 0; // TODO: set the right value
    private const float THRESHOLD = 0; // TODO: set the right value (to be tested with the robots)
    public IOManager ioManager; // TODO: instanciate this somewhere

    private bool liesWithinRange = false;
    private bool liesBeforeLine  = false;

    void Start()
    {

    }

    void Update()
    {
        
        float posX = gameObject.GetComponent<Rigidbody>().position.x;

        if(posX < LINE_POS_X - 2*THRESHOLD)
        {
            liesBeforeLine = true;
        }

        if(posX > LINE_POS_X + 2*THRESHOLD)
        {
            liesBeforeLine = false;
        }


        bool within = IsWithin(posX);

        if(!liesWithinRange && within && liesBeforeLine)
        {
            ioManager.OnStoneThrown(gameObject.GetComponent<SimulationStone>());
        }

        liesWithinRange = within;
    }


    private bool IsWithin(float posX)
    {
        return Math.Abs(posX - LINE_POS_X) < THRESHOLD; 
    }
}