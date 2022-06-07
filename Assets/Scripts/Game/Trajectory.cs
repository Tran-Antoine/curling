using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Trajectory : MonoBehaviour
{

    public IOManager ioManager; // TODO: set the value

    private float BEZIER_DISTANCE_THRESHOLD = 1f;
    private float SPEED_THRESHOLD = 0.1f;
    private float ATTAINED_THERSHOLD = 0.5f;


    public float distanceMultiplierThrow = 20f;
    public float distanceMultiplierCollision = 5f;
    public float curveMultiplier = 0.75f;

    Vector3[] points = new Vector3[4];
 
    List<Vector3[]> globalTraj = new List<Vector3[]>();


    Queue<Vector3> samplePoints = new Queue<Vector3>();

    public bool isComputed = false;

    private Vector3 finalPosition = Vector3.zero;
    

    public Trajectory(IOManager manager){
        this.ioManager = manager;
    }

    //to reset the trajectory
    public void resetTraj(Vector3 position){
        for (int i = 0; i < 4; i++)
        {
            points[i] = position;
        }
        points = new Vector3[4];
        globalTraj = new List<Vector3[]>();
        samplePoints = new Queue<Vector3>();
        isComputed = false;
        finalPosition = Vector3.zero;

    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    //mathmatical trajectory at any point in time
    public Vector3 BezierCurve(float time){

        Vector3 position = Mathf.Pow(1-time, 3)*points[0] +  
        3*Mathf.Pow(1-time, 2)*(time)*points[1] + 
        3*Mathf.Pow(time, 2)*(1-time)*points[2] +
        Mathf.Pow(time, 3)*points[3];
    
        
        return position;
    }

    //computes next position
    public Vector3 NextPosition(Vector3 currentPosition){
        if(samplePoints.Count == 0){
                isComputed = false;
            }
        if(isComputed){
            if(Vector3.Distance(currentPosition, samplePoints.Peek()) < ATTAINED_THERSHOLD){
                return samplePoints.Dequeue();
        }
            return samplePoints.Peek();
        }
        return currentPosition;
    }

    Vector3 goal = Vector3.zero;
    
    //computes the next direction according to the current position
    public Vector3 NextDirection(Vector3 currentPosition){
        if(isComputed){
            goal = NextPosition(currentPosition);
            return goal - currentPosition;
        }
        return Vector3.zero;
    }

    //main method, used to calculate the trajectory
    public void setTraj(Vector3 speed, Vector3 position, float angularVelocity, bool isThrow){
        //stops the stone if it goes too slow
        if (speed.magnitude < SPEED_THRESHOLD){
            points[0] = position;
            points[1] = position;
            points[2] = position;
            points[3] = position;

            finalPosition = position;
            isComputed = false;

            ioManager.OnThrowSimulationEnded();
            return;
        }
        else{
            finalPosition = Vector3.zero;
        }
        
        //start of the curve
        points[0] = position;
        //adjust the maximum distance so that a throw can attain the circle, but collisions aren't too fast
        points[2] = isThrow ? position + distanceMultiplierThrow * speed : position + distanceMultiplierCollision * speed;
        points[1] = points[0] + (points[2] - points[0]) * 3/4;
        points[1].y = points[2].y;

        //last point accounts for the amount of curl
        Quaternion quaternion =  angularVelocity > 0 ? Quaternion.Euler(0,-120,0) : Quaternion.Euler(0,120,0); 
        points[3] = points[2] + curveMultiplier * angularVelocity * (quaternion * points[2]);

        //keep a history of the trajectory
        globalTraj.Add(points);

        isComputed = true;
        //to optimize, only take a few of the points of the bézier curve
        takeSamplePoints();
        goal = position;
    }

    //goes to target in a straight line
    public void setTrajWithTarget(Vector3 target, Vector3 position){
        if(Vector3.Distance(target, position) < ATTAINED_THERSHOLD){
            return;
        }
            points[0] = position;
            points[1] = target;
            points[2] = target;
            points[3] = target;

         //keep a history of the trajectory
        globalTraj.Add(points);

        isComputed = true;
        //to optimize, only take a few of the points of the bézier curve
        takeSamplePoints();
        goal = position;

    }

    public void increaseCurl(Vector3 sweepingSpeed, Vector3 sweepingPosition){
        
    }

    private void takeSamplePoints(){
        Vector3 nextPoint = Vector3.zero;
        for(float i = 0f; i < 1; i+= Time.deltaTime){
            if(Vector3.Distance(nextPoint, BezierCurve(i)) > BEZIER_DISTANCE_THRESHOLD){
                samplePoints.Enqueue(BezierCurve(i));
                nextPoint = BezierCurve(i);                
                //showPoint(BezierCurve(i), Color.green);
            }
        }
        //add final point
        samplePoints.Enqueue(points[3]);
    }
    // displays a sphere at a certain point, helps with debugging
    public void showPoint(Vector3 position, Color color){
        GameObject sphere1 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere1.transform.position = position + 3*Vector3.up;
        var sphereRenderer1 = sphere1.GetComponent<Renderer>();
        sphereRenderer1.material.SetColor("_Color", color);
    }

    //returns the final position. Can be zero if it is not yet computed
    public Vector3 getFinalPosition(){
        return finalPosition;
    }
}
