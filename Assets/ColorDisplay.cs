using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ColorDisplay : MonoBehaviour
{

     public Image p1_panel;
    public TextMeshProUGUI p1;

    public Image p2_panel;
    public TextMeshProUGUI p2;

    // Start is called before the first frame update
    void Start()
    {
        p1_panel.color = Settings.p1_color;
        Color lol = p1_panel.color;
        lol.a = 100/255f;
        p1_panel.color = lol;

        p2_panel.color = Settings.p2_color;
        lol = p2_panel.color;
        lol.a = 100/255f;
        p2_panel.color = lol;

        p1.color = Settings.p1_color;
        p2.color = Settings.p2_color;
    }
}
