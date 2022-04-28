using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SetPlayerColor : MonoBehaviour
{
    public Button dst;
    // Start is called before the first frame update
    
    public Button src;


    void Awake()
     {
        src.onClick.AddListener(Clicked);
     }

    void Clicked()
     {
        var tmp = src.image.color;
        tmp.a = 1f;
        dst.image.color = tmp;
        src.image.color = tmp;

     }
}
