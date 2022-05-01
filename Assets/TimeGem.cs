using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGem : MonoBehaviour
{
    public GameObject Gem;
    private const float TIME_GEM_RESPAWN_TIME = 10.0f;
    private const float TIME_GEM_WAITING_TIME = 20.0f;
    private float gemTimer = 0f;
    private float timeToNextGem = TIME_GEM_WAITING_TIME;
    private System.Random rand = new System.Random();
    private bool firstUpdate = true;
    public AudioSource appears;

    public void Start(){
        gameObject.tag = "TimeGem";
        appears = GetComponent<AudioSource>();
    }

    private IEnumerator playSound(AudioSource sound){
        sound.Play();
        yield return new WaitForSeconds(2f);
    }

    public void UpdateBehavior(){
        if(gemTimer <= 0f && timeToNextGem <= 0f){
            if (firstUpdate) {
                float x = (float)rand.NextDouble() * (Gems.X_MAX - Gems.X_MIN) + Gems.X_MIN;
                float y = (float)rand.NextDouble() * (Gems.Y_MAX - Gems.Y_MIN) + Gems.Y_MIN;
                Gem.transform.position = new Vector3(x, Gems.Z_VAL, y) + Gem.transform.parent.transform.parent.transform.position;
                firstUpdate = false;
            } else {
                gemTimer = TIME_GEM_RESPAWN_TIME;
                timeToNextGem = TIME_GEM_WAITING_TIME;
                Gem.SetActive(true);
                StartCoroutine(playSound(appears));
            }
        }
        else if (gemTimer > 0f){
            firstUpdate = true;
            gemTimer -= Time.deltaTime;
        } else if(timeToNextGem > 0f){
            Gem.SetActive(false);
            timeToNextGem -= Time.deltaTime;
        } else{
            //should never enter
        }
        
    }

    public void caughtByPlayer(){
        Gem.SetActive(false);
        firstUpdate = true;
    }

    public void respawn(){
        Gem.SetActive(false);
        gemTimer = 0f;
        timeToNextGem = 0f;
        firstUpdate = true;
    }
}
