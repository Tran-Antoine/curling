using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStep : MonoBehaviour
{
    public GameObject tutorialManager;
    
    public void DoNextStep(){
        tutorialManager.GetComponent<TutorialManager>().nextStep();
    }

}
