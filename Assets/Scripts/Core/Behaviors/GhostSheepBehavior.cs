using System.Linq;
using UnityEngine;


public class GhostSheepBehavior : AgentBehaviour {

    private GameObject sheep;

    private GameObject[] players;
    private GameObject[] walls;
    private GameObject[] obstacles;

    private GameObject ring;

    private float obstacleRepulsion = 10F;
    private float wallRepulsion = 200F;
    private float playerRepulsion = 100F;
    private float ringRepulsion = 2F;

    public void Start() {
        sheep = gameObject;

        sheep.tag = "Sheep";     
        if (players == null){
            players = GameObject.FindGameObjectsWithTag("Player");
        }  
        if (obstacles == null){
            obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        } 
        if (walls == null){
            walls = orderUpDownLefRight(GameObject.FindGameObjectsWithTag("Wall"));
        }
        if (ring == null)
        {
            ring = GameObject.FindWithTag("Ring");
        }
    }

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        steering.linear = direction() * agent.maxAccel;

        return steering;
    }

    private GameObject[] orderUpDownLefRight(GameObject[] walls){
        GameObject upmost = walls[0];
        GameObject rightmost = walls[0];
        GameObject leftmost = walls[0];
        GameObject downmost = walls[0];

        foreach (GameObject w in walls) {
            upmost = upmost.transform.position.z > w.transform.position.z ? upmost : w;
            rightmost = rightmost.transform.position.x > w.transform.position.x ? rightmost : w;
            leftmost = leftmost.transform.position.x < w.transform.position.x ? leftmost : w;
            downmost = downmost.transform.position.z < w.transform.position.z ? downmost : w;
        }

        GameObject[] output = {upmost, downmost, leftmost, rightmost};
        return output;
    }

    private Vector3 direction() {

        Vector3 v = new Vector3(0,0,0);

        foreach (GameObject p in players) {
            v += reach(sheep, p) / sqDistance(sheep, p) * playerRepulsion;
        }
        
        v += new Vector3(0, 0, 1 / reach(sheep, walls[0]).z) * wallRepulsion;
        v += new Vector3(0, 0, 1 / reach(sheep, walls[1]).z) * wallRepulsion;
        v += new Vector3(1 / reach(sheep, walls[2]).x, 0, 0) * wallRepulsion;
        v += new Vector3(1 / reach(sheep, walls[3]).x, 0, 0) * wallRepulsion;

        // uncomment this when ring is ready
        //v += reach(sheep, ring) / sqDistance(sheep, ring) * ringRepulsion;

        foreach (GameObject o in obstacles) {
            v += reach(sheep, o) / sqDistance(sheep, o) * obstacleRepulsion;
        }

        //positive rot
        int angle = Random.Range(-60, -30);
        //negative rot

        
        return Quaternion.Euler(0, angle, 0) * v;
    }

    private Vector3 reach(GameObject a, GameObject b)
    {
        Vector3 v = a.transform.position - b.transform.position;
        return new Vector3(v.x, 0, v.z);
    }

    private float sqDistance(GameObject a, GameObject b)
    {
        float dist = Vector3.Distance(a.transform.position, b.transform.position);
        return dist * dist;
    }

    bool comp(Collision obj, string tag)
    {
        return obj.gameObject.CompareTag(tag);
    }

    void OnCollisionEnter(Collision obj){

        if(comp(obj, "Obstacle")) {
            obstacleRepulsion = 10000F;
        }
        if(comp(obj, "Wall")) {
            wallRepulsion = 10000F;
        }
        if(comp(obj, "Player")){
            playerRepulsion = 10000F;
        }

        if(comp(obj, "Ring")) {
            ringRepulsion = 10F;
        }
    }

    void OnCollisionExit(Collision obj){
        if(comp(obj, "Obstacle")){
            obstacleRepulsion = 10F;
        }
        if(comp(obj, "Wall")){
            wallRepulsion = 200F;
        }
        if(comp(obj, "Player")){
            playerRepulsion = 100F;
        }
        if(comp(obj, "Ring")){
            ringRepulsion = 2F;
        }
    }

}
