using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingTrigger : MonoBehaviour
{  
    private GameObject[] players;
    public AudioSource win;

    // Start is called before the first frame update
    void Start(){

        gameObject.tag = "Ring";
        if (players == null){
            win = GetComponent<AudioSource>();
            players = GameObject.FindGameObjectsWithTag("Player");
        }         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){

        //Debug.Log(other.transform.parent.gameObject.name + " triggers.");
        //Debug.Log("Tag of collider : " + other.attachedRigidbody.GetComponent<CelluloAgent>().tag);

        if (other.attachedRigidbody.GetComponent<CelluloAgent>().CompareTag("Sheep") && players.Length >= 1){

            float distance = float.MaxValue;
            GameObject closestPlayer = players[0];
            foreach (GameObject p in players) {
                float squaredDistance = Vector3.Distance(other.transform.position, p.transform.position) * Vector3.Distance(other.transform.position, p.transform.position);
                if (squaredDistance < distance) {
                    closestPlayer = p; 
                    distance = squaredDistance;
                    }
            }
            int score = closestPlayer.GetComponent<MoveWithKeyboardBehavior>().score + 1;
            closestPlayer.GetComponent<MoveWithKeyboardBehavior>().updateScore(score);
            win.Play();
        }
    }
}
