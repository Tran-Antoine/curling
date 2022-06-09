using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class CelluloVisualisation : MonoBehaviour
{

    public GameObject spawned_cellulos;
    public GameObject c1;
    public GameObject c2;
    public GameObject c3;

    public GameObject i_1;
    public GameObject i_2;
    public GameObject i_3;
    public GameObject i_4;
    public GameObject i_5;
    public GameObject i_6;
    public GameObject i_7;
    public GameObject i_8;
    public GameObject i_9;
    public GameObject i_10;
    public GameObject i_11;
    public GameObject i_12;

    private static CelluloAgent image1;
    private static CelluloAgent image2;
    private static CelluloAgent image3;
    private static CelluloAgent image4;
    private static CelluloAgent image5;
    private static CelluloAgent image6;
    private static CelluloAgent image7;
    private static CelluloAgent image8;
    private static CelluloAgent image9;
    private static CelluloAgent image10;
    private static CelluloAgent image11;
    private static CelluloAgent image12;
    public static CelluloVisualisation dummy;

    static List<CelluloAgent> images = new List<CelluloAgent>();


    private static CelluloAgent agent1;
    private static CelluloAgent agent2;
    private static CelluloAgent agent3;

    static LayerMask imagesLayer;
    static LayerMask stonesLayer;

    private static int i = 0;

    private static int curr = 0;
    // Start is called before the first frame update
    void Start()
    {
        agent1 = c1.GetComponent<CelluloAgent>();
        agent2 = c2.GetComponent<CelluloAgent>(); 
        agent3 = c3.GetComponent<CelluloAgent>();
        
        image1  = i_1.GetComponent<CelluloAgent>();
        image2  = i_2.GetComponent<CelluloAgent>();
        image3  = i_3.GetComponent<CelluloAgent>();
        image4  = i_4.GetComponent<CelluloAgent>();
        image5  = i_5.GetComponent<CelluloAgent>();
        image6  = i_6.GetComponent<CelluloAgent>();
        image7  = i_7.GetComponent<CelluloAgent>();
        image8  = i_8.GetComponent<CelluloAgent>();
        image9  = i_9.GetComponent<CelluloAgent>();
        image10 = i_10.GetComponent<CelluloAgent>();
        image11 = i_11.GetComponent<CelluloAgent>();
        image12 = i_12.GetComponent<CelluloAgent>();

        images.Add(image1);
        images.Add(image2);
        images.Add(image3);
        images.Add(image4);
        images.Add(image5);
        images.Add(image6);
        images.Add(image7);
        images.Add(image8);
        images.Add(image9);
        images.Add(image10);
        images.Add(image11);
        images.Add(image12);
        imagesLayer = LayerMask.NameToLayer("Images");
        stonesLayer = LayerMask.NameToLayer("Stones");

        dummy = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void MoveTo(CelluloAgent image){

        
        CelluloAgent closest = ClosestPlayer(image);
        //Destroy(agent.GetComponent<Rigidbody>());
        //agent.GetComponent<CelluloAgentRigidBody>().Delete();
        closest.SetGoalPosition(image.transform.localPosition.x, image.transform.localPosition.z, image.maxAccel);
        DeleteBodyRecursively(image.gameObject);
    }

    public static void RecallClosestCellulo(){


        var map = new Dictionary<Vector3, CelluloAgent>(); 

        map[agent1.transform.position] = agent1;
        map[agent2.transform.position] = agent2;
        map[agent3.transform.position] = agent3;

        Dictionary<Vector3, CelluloAgent>.KeyCollection distances = map.Keys;


        Vector3 closest = new Vector3(float.MaxValue, 0f, 0F);

        foreach (Vector3 vect in distances)
        {
            if (closest.x >= vect.x){
                closest = vect;
            }
            
        }
        CelluloAgent image = MoveImageToPosition(map[closest]);
        RestoreBodyRecursively(image.gameObject);

        DeleteBodyRecursively(map[closest].gameObject);
        
        dummy.StartCoroutine(RestoreWithDelay(map[closest]));
        ResetAllTraj();
        MoveStoneBackHome(map[closest]);
    }

    private static IEnumerator RestoreWithDelay(CelluloAgent stone){
            yield return new WaitForSeconds(3);
            RestoreBodyRecursively(stone.gameObject);
    }

    private static IEnumerator MoveBackHomeWithDelay(CelluloAgent stone){
            yield return new WaitForSeconds(2);
            MoveStoneBackHome(stone);
    }

    static void MoveStoneBackHome(CelluloAgent closest){
        closest.SetGoalPosition(closest.basePosition.x, closest.basePosition.z, closest.maxAccel);
    }


    static CelluloAgent ClosestPlayer(CelluloAgent agent){
        
        var map = new Dictionary<float, CelluloAgent>(); 

        float d1 = Vector3.Distance(agent1.transform.position, agent.transform.position);
        map[d1] = agent1;

        float d2 = Vector3.Distance(agent2.transform.position, agent.transform.position);
        map[d2] = agent2;

        float d3 = Vector3.Distance(agent3.transform.position, agent.transform.position);
        map[d3] = agent3;

        Dictionary<float, CelluloAgent>.KeyCollection distances = map.Keys;

        float min = float.MaxValue;

        foreach (float d in distances)
        {
            if (min >= d){
                min = d;
            }
            
        }
        
        return map[min];
    }

    void RestoreBodies(){

    }

    static void ResetAllTraj(){
        agent1.stone.ResetStone();
        agent2.stone.ResetStone();
        agent3.stone.ResetStone();

        foreach (CelluloAgent agent in images)
        {
            agent.stone.ResetStone();
        }

    }





    static CelluloAgent MoveImageToPosition(CelluloAgent agent){
        images[i].SetGoalPosition(agent.transform.localPosition.x, agent.transform.localPosition.z, agent.maxAccel);
        return images[i++];
    }

    static void MoveImageBackHome(CelluloAgent image){
        image.SetGoalPosition(image.basePosition.x, image.basePosition.z, image.maxAccel);
        ++i;
    }


    /**
    void SetInvisible(){ 
        cellulo_body.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        foreach(Transform t in cellulo_leds.transform){
            t.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        Invoke("SetVisible", 5);
    }*/

    static void DeleteBodyRecursively(GameObject o){
        o.layer = imagesLayer;
        foreach(Transform child in o.GetComponentInChildren<Transform>(true)){
            child.gameObject.layer = imagesLayer;
        }
    }

    static void RestoreBodyRecursively(GameObject o){
        o.layer = stonesLayer;
        foreach(Transform child in o.GetComponentInChildren<Transform>(true)){
            child.gameObject.layer = stonesLayer;
        }
    }

    /**foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }*/

}
