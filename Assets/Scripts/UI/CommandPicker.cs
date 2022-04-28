using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CommandPicker : MonoBehaviour
{


    public Button c1;
    public TextMeshProUGUI c1Text;

    public Button c2;
    public TextMeshProUGUI c2Text;

    public Button c3;
    public TextMeshProUGUI c3Text;

    public TextMeshProUGUI myPlayerText;
    public TextMeshProUGUI otherPlayerText;

    public String toDisplay_O;
    public String toDisplay_M;

    /**public Button otherPlayer;
    public Button myPlayer;*/

    //public  List<TextMeshProUGUI> LOL;

    public Dictionary<Button, TextMeshProUGUI> dict = new Dictionary<Button, TextMeshProUGUI>();



    // Start is called before the first frame update
    void Start()
    {
        dict.Add(c1, c1Text);
        dict.Add(c2, c2Text);
        dict.Add(c3, c3Text);

        foreach(Button ci in dict.Keys){
            dict[ci].gameObject.SetActive(false);
            dict[ci].enabled = false;
        } 
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Button ci in dict.Keys){

            if (dict[ci].text.Equals(otherPlayerText.text)){
                dict[ci].gameObject.SetActive(true);
                dict[ci].enabled = true;
                ci.enabled = false;

                var tmp = ci.image.color;
                tmp.a = 0.5f;
                ci.image.color = tmp; 
                
            } else if (dict[ci].text.Equals(myPlayerText.text)){
                dict[ci].gameObject.SetActive(true);
                dict[ci].enabled = true;
                ci.enabled = true;
            } else {
                dict[ci].gameObject.SetActive(true);
                dict[ci].enabled = true;
                ci.enabled = true;
                
                var tmp = ci.image.color;
                tmp.a = 1f;
                ci.image.color = tmp; 
            }   
        }
    }
}
