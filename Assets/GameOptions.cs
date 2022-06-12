using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using System.Globalization;


public class GameOptions : MonoBehaviour
{

    public ToggleGroup trajGroup;
    public ToggleGroup curlGroup;

    public void ChooseTrajectory(){
        Toggle choice = trajGroup.ActiveToggles().FirstOrDefault();
        if (choice != null) {
            string text = choice.GetComponentInChildren<TextMeshProUGUI>().text;
            Settings.showTrajectory = String.Equals(text, "show");

} 
    }

    public void ChooseCurl(){
        Toggle choice = curlGroup.ActiveToggles().FirstOrDefault();
        if (choice != null) {
            string text = choice.GetComponentInChildren<TextMeshProUGUI>().text;
            Settings.curlsIncluded = String.Equals(text, "yes");
            } 
    } 
}
