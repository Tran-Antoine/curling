using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetPlayerCommand : MonoBehaviour
{
    public TextMeshProUGUI dst;
    public TextMeshProUGUI src;

    // Start is called before the first frame update
    
    public Button trigger;


    void Awake()
     {
        trigger.onClick.AddListener(Clicked);
     }

    void Clicked()
     {
        dst.text = src.text;
     }
}
