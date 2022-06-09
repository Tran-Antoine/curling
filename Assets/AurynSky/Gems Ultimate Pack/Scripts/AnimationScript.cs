using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


public class AnimationScript : MonoBehaviour {

    public bool isAnimated = false;

    public bool isRotating = false;
    public bool isFloating = false;
    public bool isScaling = false;

    public Vector3 rotationAngle;
    public float rotationSpeed;

    public float floatSpeed;
    private bool goingUp = true;
    public float floatRate;
    private float floatTimer;
   
    public Vector3 startScale;
    public Vector3 endScale;

    private bool scalingUp = true;
    public float scaleSpeed;
    public float scaleRate;
    private float scaleTimer;

    public CelluloAgent cellulo;

    public GameObject cellulo_body;
    public GameObject cellulo_leds;
    private static Color SELECTED_COLOR = new Color(189/256f, 18/256f, 18/256f);
    private static Color BASE_COLOR = new Color(33/256f, 57/256f, 108/256f);

    public static float INVISIBILITY_TIME = 5f;
    public float invisibility = 0f;

    public bool image;




	// Use this for initialization
	void Start () {
        isAnimated = true;
        isRotating = true;
        cellulo = GetComponentInParent<CelluloAgent>();
        //Debug.Log("cellulooo " + cellulo);
	}

    void OnMouseDown(){
        if (image){ 
            //cellulo.SetVisualEffect(VisualEffect.VisualEffectConstAll, SELECTED_COLOR, 1);
            //cellulo.SetGoalPosition(cellulo.basePosition.x, cellulo.basePosition.z, cellulo.maxAccel);
            //SetGoalPosition(agent.transform.localPosition.x, agent.transform.localPosition.z, toMove.maxAccel);
            //DeleteBody();
            CelluloVisualisation.MoveTo(cellulo);
            //Invoke("RestoreBody", 3);
        }
        //Invoke("SetInvisible", 5);
    }
	
    
    


    void SetVisible(){ 
        cellulo_body.GetComponent<MeshRenderer>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        foreach(Transform t in cellulo_leds.transform){
            t.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        cellulo.SetVisualEffect(VisualEffect.VisualEffectConstAll, BASE_COLOR, 1);
    }

	// Update is called once per frame
	void Update () {        
        if(isAnimated)
        {
            if(isRotating)
            {
                transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);
            }

            if(isFloating)
            {
                floatTimer += Time.deltaTime;
                Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
                transform.Translate(moveDir);

                if (goingUp && floatTimer >= floatRate)
                {
                    goingUp = false;
                    floatTimer = 0;
                    floatSpeed = -floatSpeed;
                }

                else if(!goingUp && floatTimer >= floatRate)
                {
                    goingUp = true;
                    floatTimer = 0;
                    floatSpeed = +floatSpeed;
                }
            }

            if(isScaling)
            {
                scaleTimer += Time.deltaTime;

                if (scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, endScale, scaleSpeed * Time.deltaTime);
                }
                else if (!scalingUp)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, startScale, scaleSpeed * Time.deltaTime);
                }

                if(scaleTimer >= scaleRate)
                {
                    if (scalingUp) { scalingUp = false; }
                    else if (!scalingUp) { scalingUp = true; }
                    scaleTimer = 0;
                }
            }
        }
	}
}
