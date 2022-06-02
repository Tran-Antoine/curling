using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stone : AgentBehaviour
{   

    public bool wasCollided = false;
    // Start is called before the first frame update

    private Trajectory traj = new Trajectory();

    private float time = 0f;

    private const float drag = 0.1f;


    private GameObject stone;

    private CelluloAgent cellulo;
    private Rigidbody rigidBody;

    void Start()
    {
        stone = gameObject;
        stone.tag = "Stone";

        if(cellulo == null){
            cellulo = stone.GetComponent<CelluloAgent>();
        }
        if(rigidBody == null){
            rigidBody = stone.GetComponent<Rigidbody>();
        }
        if(traj == null){
            traj = new Trajectory();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(traj.isComputed){
            time += Time.deltaTime;
        }      
    }

    //TODO add rotation
    public override Steering GetSteering(){
        Steering steering = new Steering();

        steering.linear = direction() * cellulo.maxAccel - DRAG * time*time;
        return steering;
    }

    private Vector3 direction(){

        //test stone throw
        if(!traj.isComputed && Time.time > 0 && rigidBody.position.x < 8f){
            throwStone(new Vector3(0.85f, 0f, -0.25f), rigidBody.position, -0.40f, true);
            return Vector3.zero;
        }
        else{
            return traj.NextDirection(time, rigidBody.position);
        }
    }

    bool comp(Collision obj, string tag)
    {
        return obj.gameObject.CompareTag(tag);
    }

    public void throwStone(Vector3 velocity, Vector3 start, float angMom){
        gameObject.GetComponent<Collider>().enabled = true;
        traj.resetTraj(Rigidbody.position);
        traj.setTraj(velocity, start, angMom, true);
    }

    //returns the x coordinate of the throw => idea : show it on screen
    public float showThrowDistance(){
        return traj.finalPosition.x;
    }

    public void sweep(Vector3 speed, Vector3 direction){
        traj.increaseCurl(speed, direction);
    }

    public void impactStone(Vector3 velocity, Vector3 start, float angMom){
        gameObject.GetComponent<Collider>().enabled = true;
        traj.setTraj(velocity, start, angMom, false);
    }

    private void OnCollisionEnter(Collision other) {
        if(comp(other, "Stone")){
            Stone stone2 = other.gameObject.GetComponent<Stone>();
            if(wasCollided){
                wasCollided = false;
                return;
            }
            collideWith(stone2);
        }
        if(comp(other, "Wall")){
            stopStone(getPosition());
        }
    }

    public void stopStone(Vector3 position){
        traj.setTrajWithTarget(position, position);
    }

    public Vector3 getPosition(){
        return rigidBody.position;
    }

    public Vector3 getDirection(){
        return rigidBody.velocity != null ? rigidBody.velocity : Vector3.zero;
    }

    //returns in a straight line to start. Collisions are disabled  
    public void returnToStart(Vector3 start){
        gameObject.GetComponent<Collider>().enabled = false;
        traj.setTrajWithTarget(start, rigidBody.position);
    }

    private void collideWith(Stone s2){
        //Source : https://en.wikipedia.org/wiki/Elastic_collision#Two-dimensional

        
        float X1_x = rigidBody.position.x;
        float X2_x = s2.getPosition().x;
        float X1_z = rigidBody.position.z;
        float X2_z = s2.getPosition().z;
    
        Vector3 direction = getDirection();
        
        float Alpha1= Mathf.Atan2(X2_z-X1_z , X2_x-X1_x);
        float Beta1=Mathf.Atan2(direction.z,direction.x);
        float theta = Alpha1- Beta1;

        float theta1 = Mathf.Atan2(Mathf.Sin(theta), 1+Mathf.Cos(theta));
    
        float theta2 = (Mathf.PI-theta)/2;

        float V1mag = direction.magnitude*Mathf.Sqrt(1+Mathf.Cos(theta)/2);
        float V2mag = direction.magnitude*Mathf.Sin(theta/2);

        Vector3 V1 = V1mag * new Vector3(Mathf.Cos(theta1), 0, Mathf.Sin(theta1));
        Vector3 V2 = V2mag * new Vector3(Mathf.Cos(theta2), 0, Mathf.Sin(theta2));

        //avoid computation by both side
        s2.wasCollided = true;

        impactStone(V2, rigidBody.position, 0, false);
        s2.impactStone(V1, s2.getPosition(), 0, false);
    }
}
