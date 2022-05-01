using System.Linq;
using UnityEngine;


public class GhostSheepBehavior : AgentBehaviour {

    private GameObject sheep;

    private GameObject[] players;
    private GameObject[] walls;
    private GameObject[] obstacles;

    private GameObject ring;

    private float obstacleRepulsion = 10F;
    private float wallRepulsion = 60F;
    private float playerRepulsion = 120F;
    private float ringRepulsion = 1F;

    private const float CHASING_SPEED = 5F;

    private int latency = 0;


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

        if(sheep.CompareTag("Ghost"))
        {
            return toClosestPlayerDirection();
        }
        
        if(sheep.CompareTag("Sheep"))
        {
            return runAwayDirection();
        }
        
        return new Vector3(0, 0, 0);
    }

    private Vector3 runAwayDirection()
    {
        Vector3 v = new Vector3(0, 0, 0);

        foreach (GameObject p in players)
        {
            v += reach(sheep, p) / sqDistance(sheep, p) * playerRepulsion;
        }

        v += new Vector3(0, 0, 1 / reach(sheep, walls[0]).z) * wallRepulsion;
        v += new Vector3(0, 0, 1 / reach(sheep, walls[1]).z) * wallRepulsion;
        v += new Vector3(1 / reach(sheep, walls[2]).x, 0, 0) * wallRepulsion;
        v += new Vector3(1 / reach(sheep, walls[3]).x, 0, 0) * wallRepulsion;

        v += reach(sheep, ring) / sqDistance(sheep, ring) * ringRepulsion;

        //positive rot
        int angle = Random.Range(-50, -40);
        //negative rot

        return Quaternion.Euler(0, angle, 0) * v;
    }


    private Vector3 toClosestPlayerDirection()
    {
        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject p in players)
        {
            float distance = distanceTo(sheep, p);
            if(distance < closestDistance)
            {
                closestPlayer = p;
                closestDistance = distance;
            }
        }

        if(closestPlayer == null || latency > 0)
        {
            latency -=1;
            return new Vector3(0, 0, 0);
        }

        return reach(closestPlayer, sheep).normalized * CHASING_SPEED;
    }

    private Vector3 reach(GameObject a, GameObject b)
    {
        Vector3 v = a.transform.position - b.transform.position;
        return new Vector3(v.x, 0, v.z);
    }

    private float distanceTo(GameObject a, GameObject b)
    {
        return reach(a, b).magnitude;
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
            latency = 40;
        }

        if(comp(obj, "Ring")) {
            ringRepulsion = 10F;
        }
        if(comp(obj, "SpaceGem")) {
            SpaceGem gem = obj.gameObject.GetComponent<SpaceGem>();
            gem.respawn();
        }
        if(comp(obj, "TimeGem")) {
            TimeGem gem = obj.gameObject.GetComponent<TimeGem>();
            gem.respawn();
        }}

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
