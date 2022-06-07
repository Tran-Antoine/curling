using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;
using System.Globalization;


public class ToggleGroupTurns : MonoBehaviour
{
    ToggleGroup group;
    public int turns = 0;

    void Start(){
        group = GetComponent<ToggleGroup>();
    }

    public void Choose(){
        Toggle choice = group.ActiveToggles().FirstOrDefault();
        if (choice != null) {
            turns = Int16.Parse(choice.GetComponentInChildren<TextMeshProUGUI>().text);
            } 
    }

}

