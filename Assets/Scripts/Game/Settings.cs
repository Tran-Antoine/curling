using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{

    public static Color p1_color = new Color(66/256f, 135/256f, 60/256f);
    public static Color p2_color = new Color(204/256f, 155/256f, 45/256f);

    public Button p1_button;
    public Button p2_button;

    //public static InputKeyboard p1_keyboard = InputKeyboard.wasd;
    //public static InputKeyboard p2_keyboard = InputKeyboard.arrows;

    public TextMeshProUGUI p1_control;
    public TextMeshProUGUI p2_control;


    void Update() {

        p1_color = p1_button.image.color;
        p2_color = p2_button.image.color;

        /*switch (p1_control.text)
        {
            case "ARROWS" : 
                p1_keyboard = InputKeyboard.arrows;
                break;

            case "MOUSE" : 
                p1_keyboard = InputKeyboard.mouse;
                break;

            case "WASD" : 
                p1_keyboard = InputKeyboard.wasd;
                break;
        }

        switch (p2_control.text)
        {
            case "ARROWS" : 
                p2_keyboard = InputKeyboard.arrows;
                break;

            case "MOUSE" : 
                p2_keyboard = InputKeyboard.mouse;
                break;

            case "WASD" : 
                p2_keyboard = InputKeyboard.wasd;
                break;
        }*/
    }
}
