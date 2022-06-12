using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurlToggle : MonoBehaviour
{
    public Toggle curlToggle;
    // Start is called before the first frame update
    void Start()
    {
        curlToggle.isOn = Settings.curlsIncluded;
    }

    public void onChangedValue(){
        if (!curlToggle.isOn){
            Settings.curlsIncluded = false;
        }
        if(curlToggle.isOn){
            Settings.curlsIncluded = true;
        }
    }
}
