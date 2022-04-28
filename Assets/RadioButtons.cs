using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using System.Globalization;


public class RadioButtons : MonoBehaviour
{
    ToggleGroup group;
    private static float CHOSEN_TIME = 120.0f;

    void Start(){
        group = GetComponent<ToggleGroup>();
    }

    public void Choose(){
        Toggle choice = group.ActiveToggles().FirstOrDefault();
        if (choice != null) {
            computeText(choice.GetComponentInChildren<TextMeshProUGUI>().text);
            } 
    }

    private void computeText(string s){
        string[] subStrings = s.Split(':');
        float minutes = (float) Convert.ToDouble(subStrings[0]);
        float seconds = (float) Convert.ToDouble(subStrings[1]);
        CHOSEN_TIME = minutes * 60.0f + seconds;
    }

    public static float getTime() {
        return CHOSEN_TIME;
    }
}
