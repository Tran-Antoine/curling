using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TrajToggle : MonoBehaviour
{
    public Toggle trajToggle;

    // Start is called before the first frame update
    void Start()
    {
        trajToggle.isOn = Settings.showTrajectory;
    }

    // Update is called once per frame
    public void onChangedValue(){
        if (!trajToggle.isOn){
            Settings.showTrajectory = false;
        }
        if(trajToggle.isOn){
            Settings.showTrajectory = true;
        }
    }
}
