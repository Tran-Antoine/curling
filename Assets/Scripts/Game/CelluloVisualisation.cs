using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelluloVisualisation : MonoBehaviour
{

    public GameObject spawned_cellulos;
    public GameObject c1;
    public GameObject c2;

    private static CelluloAgent agent1;
    private static CelluloAgent agent2;

    public GameObject toThrow;

    private static int curr = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent1 = c1.GetComponent<CelluloAgent>();
        agent2 = c2.GetComponent<CelluloAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void MoveTo(CelluloAgent agent){
        CelluloAgent toMove = NextPlayer();
        //Destroy(agent.GetComponent<Rigidbody>());
        //agent.GetComponent<CelluloAgentRigidBody>().Delete();
        toMove.SetGoalPosition(agent.transform.localPosition.x, agent.transform.localPosition.z, toMove.maxAccel);
    }


    static CelluloAgent NextPlayer(){
        if (curr++%2 == 0) return agent1; 
        else return agent2;
    }
}
