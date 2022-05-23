using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Windows.Input;

public class FirstPersonView : MonoBehaviour
{

    Vector3 position;

    private static float baseAccel = .2f;
    private static float fastAccel = .45f;


    private float accel;
    float xmvt;
    float zmvt;


    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0,0,0);   
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetKeyDown(KeyCode.L));
        accel = Input.GetKey(KeyCode.LeftShift) ? fastAccel : baseAccel;
        
        xmvt = Input.GetAxis("VerticalWASD") * accel;
        zmvt = -Input.GetAxis("HorizontalWASD") * accel;

        Vector3 direction = new Vector3(xmvt, 0, zmvt);


        transform.position += direction;
        
    }
}
