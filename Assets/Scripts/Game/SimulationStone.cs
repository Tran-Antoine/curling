using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationStone : AgentBehaviour
{   
    public AudioSource collisionSound;
    private StaticStone logicStone;

    //public bool isThrown = false;
    // Start is called before the first frame update

    private Trajectory traj;

    private float time = 0f;

    private const float drag = 0.001f;

    public IOManager manager;

    
    List<Vector3> positions = new List<Vector3>();
    Vector3 speed;
    float angVel = 0f;
    List<Quaternion> rotations = new List<Quaternion>();
    List<float> averageAngVel = new List<float>();


    private GameObject stone;

    public CelluloAgent cellulo;

    private bool thrown = false;

    private bool isFinished = true;


    public void SetThrown(bool b){
        thrown = b;
    }
    public StaticStone GetLogicStone()
    { 
        return logicStone; 
    }

    public void SetLogicStone(StaticStone logicStone)
    { 
        this.logicStone = logicStone;
    }

   
    public bool image;


    void Start()
    {
        this.logicStone = new StaticStone(Vector3.zero);
        stone = gameObject;
        
        stone.tag = "Stone";

        if(cellulo == null){
            cellulo = stone.GetComponentInParent<CelluloAgent>();
        }
        
        cellulo.SetStone(this);

        if(traj == null){
            traj = new Trajectory(manager, this);
        }
        cellulo.SetCasualBackdriveAssistEnabled(true);

        //cellulo.SetHapticBackdriveAssistEnabled(true);

        //positions = Enumerable.Repeat(transform.position, 100).ToList();
        //rotations = Enumerable.Repeat(transform.rotation, 20).ToList();
        //averageAngVel = Enumerable.Repeat(0f, 20).ToList();

    }

    

    Vector3 previousDir;

    Vector3 previousPos;


    Vector3 throwPos;
    Vector3 initPos;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        if(traj.isComputed){
            time += Time.deltaTime;
        } 
        this.logicStone.SetPosition(transform.position);

        
        
        //speed = Vector3.Lerp(transform.position, previousPos, 0.1f);
        

        /**
        speed = (cellulo.transform.position - previousPos)/(6*Time.fixedDeltaTime);
        Debug.Log("Cellulo : " + cellulo + ", Pos = " + cellulo.transform.position+ ", prevPos = " + previousPos);
        Debug.Log("Cellulo : " + cellulo + "Speed = " + speed);

        
        speed.y = 0;

        if (count == 0){
            previousPos = cellulo.transform.position;
            count = 6;
            return;
        }
        
        --count;*/
        
        //previousPos = transform.position;


        //speed = (positions.Last() - positions.First())/(Time.deltaTime*positions.Count);

        /**
        positions.RemoveAt(0);
        positions.Add(transform.position);
      
        
        averageAngVel.RemoveAt(0);
        averageAngVel.Add(Quaternion.Angle(rotations.First(), rotations.Last())/(Time.deltaTime*rotations.Count)*Mathf.Deg2Rad);

        rotations.RemoveAt(0);
        rotations.Add(transform.rotation);
        */

        if(!traj.isComputed)
            //{angVel = averageAngVel.Average();}

        if(isFinished){
            ResetStone();
        }      
        
        
     
    }



    public override Steering GetSteering(){
        Steering steering = new Steering();

        steering.linear = direction().normalized * cellulo.maxSpeed;

        //steering.angular = angVel * cellulo.maxAngularAccel;
        
        return steering;
    }

    private Vector3 direction(){
        return traj.NextDirection(transform.position);
    }

    bool comp(Collision obj, string tag)
    {
        return obj.gameObject.CompareTag(tag);
    }

    public void ThrowStoneFromCurrentVelocities()
    {     
        speed = (cellulo.transform.position - throwPos)/0.40f;

        if(Settings.curlsIncluded){
           angVel = (Vector3.SignedAngle(initPos, cellulo.transform.position, Vector3.right))/2.25f;
        }
        
        
        ThrowStone(speed, transform.position, angVel);
    }

    public void ThrowStoneWithFixedCurl(Vector3 speedVector)
    {    
        traj.setTraj(4 * new Vector3(1f, 0f, 0.25f), transform.position, -0.5f, true);
    }

    public void RegisterPosition(){
        throwPos = transform.position;
    }

    public void RegisterAngle(){
        initPos = transform.position;
    }


    public void ThrowStone(Vector3 velocity, Vector3 start, float angVelThrow){
        
        //averageAngVel = Enumerable.Repeat(angVelThrow, 20).ToList();
        angVel = angVelThrow;
        isFinished = false;
        time = 0f;
        StartCoroutine(DeleteTrajectorysSpheres());
        traj.setTraj(velocity, start, angVel, true);
        thrown = true;
    }

    //returns the x coordinate of the throw => idea : show it on screen
    /**public float showThrowDistance(){
        return traj.getFinalPosition().x;
    }*/

    private void OnCollisionEnter(Collision other) {
        collisionSound.Play();
        if(comp(other, "Stone")){
           if (thrown){ 
                SimulationStone stone2 = other.gameObject.GetComponent<SimulationStone>();
                //Debug.Log("moi, caillou " + this + " est rentr?? dans : " + stone2);
                
                collideWith(stone2);
                collisionSound.Play();
            }
        }
        if(comp(other, "Wall")){
            OnReached();
        }
    }

    public Vector3 getPosition(){
        return transform.position;
    }

    public Vector3 getSpeed(){
        return speed != null ? speed : Vector3.zero;
    }


    //returns in a straight line to start. Collisions are disabled  
    public void returnToStart(Vector3 start){
        isFinished = false;
        traj.setTrajWithTarget(start, transform.position);
    }

     private void collideWith(SimulationStone s2){
        //Source : https://en.wikipedia.org/wiki/Elastic_collision#Two-dimensional

            
            float X1_x = transform.position.x;
            float X2_x = s2.getPosition().x;
            float X1_z = transform.position.z;
            float X2_z = s2.getPosition().z;

            
            Vector3 speed = getSpeed();
            //Debug.Log("Speed : " + getSpeed());

            traj.resetTraj(transform.position);
            s2.traj.resetTraj(s2.transform.position);

            float Alpha = Mathf.Atan2(X2_z-X1_z , X2_x-X1_x);
            float Beta = Mathf.Atan2(speed.z, speed.x);
            
            float theta = Alpha-Beta;


            float theta1 = theta/2;

            float theta2 = -(Mathf.PI-theta)/2;

            float V1mag = speed.magnitude*Mathf.Cos(theta/2);
            float V2mag = speed.magnitude*Mathf.Sin(theta/2);
            

            Vector3 V1 = V1mag *  new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1));
            Vector3 V2 = V2mag * new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2));
            
            //impactStone(V2, transform.position, angVel/2f);

            impactStone(V2, transform.position);
            s2.impactStone(V1, s2.getPosition());
        
    }


    private IEnumerator DeleteTrajectorysSpheres(){
        yield return new WaitForSeconds(10f);
        traj.DeleteTrajectorysSpheres();
    }

    public void impactStone(Vector3 speed, Vector3 start){
        
        //averageAngVel = Enumerable.Repeat(angVelImpact, 20).ToList();
        //angVel = angVelImpact;
        isFinished = false;
        StartCoroutine(DeleteTrajectorysSpheres());
        traj.setTraj(speed, start, 0f, false);
        
    }

    public void OnReached(){
        if(thrown && manager != null){manager.OnThrowSimulationEnded();}
        ResetStone();
        isFinished = true;
    }

    public bool getIsFinished(){
        return isFinished;
    }

    public void ResetStone(){
        time = 0f;
        thrown = false;
        traj.resetTraj(transform.position);

        positions = Enumerable.Repeat(transform.position, 100).ToList();
        rotations = Enumerable.Repeat(transform.rotation, 20).ToList();
        averageAngVel = Enumerable.Repeat(0f, 20).ToList();
        angVel = 0f;
        
    }

}
