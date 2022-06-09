using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationStone : AgentBehaviour
{   

    private StaticStone logicStone;

    //public bool isThrown = false;
    // Start is called before the first frame update

    private Trajectory traj;

    private float time = 0f;

    private const float drag = 0.1f;

    public IOManager manager;


    private GameObject stone;

    public CelluloAgent cellulo;
    private Rigidbody rigidBody;
    private bool first = true;

    private bool thrown = false;

    public void SetThrown(bool b){
        thrown = b;
    }
    public StaticStone GetLogicStone()
    { 
        return logicStone; 
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

        if(rigidBody == null){
            rigidBody = stone.GetComponent<Rigidbody>();
        }

        if(traj == null){
            traj = new Trajectory(manager, this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(traj.isComputed){
            time += Time.deltaTime;
        } 
        this.logicStone.SetPosition(rigidBody.position); 

        traj.PrintTraj();   
    }

    //TODO add rotation
    public override Steering GetSteering(){
        Steering steering = new Steering();

        steering.linear = direction() * Mathf.Max(cellulo.maxAccel - drag*time*time, 0);
        return steering;
    }

    private Vector3 direction(){
        return traj.NextDirection(rigidBody.position);
    }

    bool comp(Collision obj, string tag)
    {
        return obj.gameObject.CompareTag(tag);
    }

    public void ThrowStoneFromCurrentVelocities()
    {
        ThrowStone(rigidBody.velocity, rigidBody.position, rigidBody.angularVelocity.magnitude);
    }
    public void ThrowStone(Vector3 velocity, Vector3 start, float angMom){
        time = 0f;
        traj.setTraj(velocity, start, angMom, true);
        thrown = true;
    }

    //returns the x coordinate of the throw => idea : show it on screen
    /**public float showThrowDistance(){
        return traj.getFinalPosition().x;
    }*/

    public void sweep(Vector3 speed, Vector3 direction){
        traj.increaseCurl(speed, direction);
    }

    private void OnCollisionEnter(Collision other) {
        if(comp(other, "Stone")){
           if (thrown){ 
                SimulationStone stone2 = other.gameObject.GetComponent<SimulationStone>();
                Debug.Log("moi, caillou " + this + " est rentré dans : " + stone2);
                
                collideWith(stone2);
            }
        }
        if(comp(other, "Wall")){
            OnReached();
        }
    }

    public Vector3 getPosition(){
        return rigidBody.position;
    }

    public Vector3 getSpeed(){
        return rigidBody.velocity != null ? rigidBody.velocity : Vector3.zero;
    }

    //returns in a straight line to start. Collisions are disabled  
    public void returnToStart(Vector3 start){
        traj.setTrajWithTarget(start, rigidBody.position);
    }

    private void collideWith(SimulationStone s2){
        //Source : https://en.wikipedia.org/wiki/Elastic_collision#Two-dimensional

        

            float X1_x = rigidBody.position.x;
            float X2_x = s2.getPosition().x;
            float X1_z = rigidBody.position.z;
            float X2_z = s2.getPosition().z;

            Vector3 speed = getSpeed();


            traj.resetTraj(rigidBody.position);
            s2.traj.resetTraj(s2.rigidBody.position);

            float Alpha1 = Mathf.Atan2(X2_z-X1_z , X2_x-X1_x);
            
            float theta = Alpha1;


            float theta1 = Mathf.Atan2(Mathf.Sin(theta), 1+Mathf.Cos(theta));

            float theta2 = (Mathf.PI-theta)/2;

            float V1mag = speed.magnitude*Mathf.Sqrt(1+Mathf.Cos(theta)/2);
            float V2mag = speed.magnitude*Mathf.Sin(theta/2);
            
            Vector3 V1 = V1mag *  new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1));
            Vector3 V2 = -V2mag * new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2));
            
            impactStone(V2, rigidBody.position);
            s2.impactStone(V1, s2.getPosition());
        
    }

    public void impactStone(Vector3 speed, Vector3 start){
        traj.setTraj(speed, start, 0, false);
        
    }

    public void OnReached(){
        if(thrown){manager.OnThrowSimulationEnded();}
        ResetStone();
    }

    public void ResetStone(){
        time = 0f;
        thrown = false;
        traj.resetTraj(rigidBody.position);
    }
}
