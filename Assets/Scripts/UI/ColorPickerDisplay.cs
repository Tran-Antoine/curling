using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ColorPickerDisplay : MonoBehaviour
{

    public String myPlayerText;
    public String otherPlayerText;

    public Button c1;
    public TextMeshProUGUI c1Text;

    public Button c2;
    public TextMeshProUGUI c2Text;

    public Button c3;
    public TextMeshProUGUI c3Text;

    public Button c4;
    public TextMeshProUGUI c4Text;

    public Button c5;
    public TextMeshProUGUI c5Text;

    public Button c6;
    public TextMeshProUGUI c6Text;

    public Button c7;
    public TextMeshProUGUI c7Text;

    public Button c8;
    public TextMeshProUGUI c8Text;

    public Button c9;
    public TextMeshProUGUI c9Text;

    public Button otherPlayer;
    public Button myPlayer;

    public Dictionary<Button, TextMeshProUGUI> dict = new Dictionary<Button, TextMeshProUGUI>();

    // Start is called before the first frame update
    void Start()
    {   

        dict.Add(c1, c1Text);
        dict.Add(c2, c2Text);
        dict.Add(c3, c3Text);
        dict.Add(c4, c4Text);
        dict.Add(c5, c5Text);
        dict.Add(c6, c6Text);
        dict.Add(c7, c7Text);
        dict.Add(c8, c8Text);
        dict.Add(c9, c9Text); 

        foreach(Button ci in dict.Keys){
            dict[ci].gameObject.SetActive(false);
            dict[ci].enabled = false;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Button ci in dict.Keys){
            var tmpCi = ci.image.color;
            tmpCi.a = 1f;

            if (tmpCi == otherPlayer.image.color){
                dict[ci].gameObject.SetActive(true);
                dict[ci].enabled = true;
                dict[ci].text = otherPlayerText;
                ci.enabled = false;
                var tmp = otherPlayer.image.color;
                tmp.a = 0.5f;
                ci.image.color = tmp; 
                
            } else if (tmpCi == myPlayer.image.color){
                dict[ci].gameObject.SetActive(true);
                dict[ci].enabled = true;
                dict[ci].text = myPlayerText; 
            } else {
                dict[ci].gameObject.SetActive(false);
                dict[ci].enabled = false;
                ci.image.color = tmpCi;
                ci.enabled = true;
            }   
        }
    }
}
