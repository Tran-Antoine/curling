using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Windows.Input;

public class CameraMovement : MonoBehaviour
{

    Vector3 position;

    private static float baseAccel = .2f;
    private static float fastAccel = .45f;

    private float rotation_speed = 15f;

    private float accel;
    float xmvt;
    float zmvt;


    private static Vector3 initialPosition;
    private Quaternion initialRotation;

    public Button reset;

    void Awake(){
        reset.onClick.AddListener(resetView);
    }


    // Start is called before the first frame update
    void Start()
    {
        position = new Vector3(0,0,0);   
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Input.GetKeyDown(KeyCode.L));
        if(Input.GetMouseButton(0)){
            transform.eulerAngles += rotation_speed * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"));
        }
        accel = Input.GetKey(KeyCode.LeftShift) ? fastAccel : baseAccel;
        
        xmvt = Input.GetAxis("VerticalWASD") * accel;
        zmvt = Input.GetAxis("HorizontalWASD") * accel;

        Vector3 RIGHT = transform.TransformDirection(Vector3.right);
        Vector3 FORWARD = transform.TransformDirection(Vector3.forward);

        transform.localPosition += RIGHT * zmvt;
        transform.localPosition += FORWARD * xmvt;

        //Vector3 direction = new Vector3(xmvt, 0, zmvt);


        //transform.position += direction;
        
    }

    void resetView(){
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}