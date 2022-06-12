using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public GameObject straightThrowInstruction;
    public GameObject curlInstruction;
    public GameObject takeOutInstruction;

    public GameObject straightThrowConclusion;
    public GameObject curlConclusion;
    public GameObject takeOutConclusion;

    public GameObject playerStone;
    private SimulationStone simStone;
    private int currentStep = 0;

    public GameObject targetStone;
    private SimulationStone simTargetStone;

    private float STRAIGHT_THRESHOLD = 0.1f;

    private bool straightFinished = false, curlFinished = false, takeOutFinished = false;
    private bool straightStarted = false, curlStarted = false, takeOutStarted = false, returnStarted = false, goToTarget = false;

    
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        simStone = playerStone.GetComponent<SimulationStone>();
        startPosition = simStone.getPosition();

        simTargetStone = targetStone.GetComponent<SimulationStone>();
    }

    // Update is called once per frame
    void Update()
    {  
        switch(currentStep){
            case 0: 
                straightThrowInstruction.SetActive(true);
                straightStarted = true;
                break;
            case 1:
                StraightThrow();
                break;
            case 2:
                straightThrowConclusion.SetActive(false);
                curlInstruction.SetActive(true);
                returnToStart();
                curlStarted = true;
                break;
            case 3: 
                CurlThrow();   
                break; 
            case 4:
                curlConclusion.SetActive(false);
                takeOutInstruction.SetActive(true);
                takeOutStarted = true;

                returnToStart();
                if(goToTarget){
                    goToTarget = false;
                    simTargetStone.returnToStart(new Vector3(25.9f, -1.5f, -0.1f));
                }



                break;
            case 5:
                TakeOut();
                break;

            default: return;
        }
    }

    void StraightThrow(){
        if(straightStarted){
            simStone.ThrowStone(4f*Vector3.right, simStone.getPosition(), 0f);
            straightStarted = false;
        }
        if(simStone.getIsFinished()){
            returnStarted = true;
            straightThrowInstruction.SetActive(false);
            straightThrowConclusion.SetActive(true);
        }
    }
    void CurlThrow(){
        if(curlStarted){

            simStone.ThrowStoneWithFixedCurl(3.75f*(new Vector3(0.85f, 0f,0.25f)));
            curlStarted = false;
        }
        if(simStone.getIsFinished()){
            returnStarted = true;
            curlInstruction.SetActive(false);
            curlConclusion.SetActive(true);
            returnStarted = true;
            goToTarget = true;
        }
    }
    void TakeOut(){
        if(takeOutStarted){
            simStone.ThrowStone(4*new Vector3(1.25f, 0f,0f), simStone.getPosition(), 0.0f);
            takeOutStarted = false;
        }
        if(simStone.getIsFinished()){
            takeOutInstruction.SetActive(false);
            takeOutConclusion.SetActive(true);
        }
        
    }


    public void nextStep(){
        if(simStone.getIsFinished()){
        currentStep = (currentStep + 1)%6; }
    }

    private void returnToStart(){
        if(returnStarted){
            simStone.returnToStart(startPosition);
            returnStarted = false;
        }
    }
}
