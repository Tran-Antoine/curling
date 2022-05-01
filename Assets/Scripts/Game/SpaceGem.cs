using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceGem : MonoBehaviour{

    public GameObject Gem;
    private const float GEM_RESPAWN_TIME = 8.0f;
    private const float WAITING_TIME = 15.0f;
    private float gemTimer = 0f;
    private float timeToNextGem = WAITING_TIME;
    private System.Random rand = new System.Random();
    private bool firstUpdate = true;
    private bool playerHasGem = false;
    public AudioSource appears;

    public void Start(){
        gameObject.tag = "SpaceGem";
        appears = GetComponent<AudioSource>();
    }

    private IEnumerator playSound(AudioSource sound){
        sound.Play();
        yield return new WaitForSeconds(2f);
    }

    public void UpdateBehavior(){
        if (playerHasGem) return;
        if(gemTimer <= 0f && timeToNextGem <= 0f){
            if (firstUpdate) {
                float x = (float)rand.NextDouble() * (Gems.X_MAX - Gems.X_MIN) + Gems.X_MIN;
                float y = (float)rand.NextDouble() * (Gems.Y_MAX - Gems.Y_MIN) + Gems.Y_MIN;
                Gem.transform.position = new Vector3(x, Gems.Z_VAL, y) + Gem.transform.parent.transform.parent.transform.position;
                firstUpdate = false;
            } else {
                gemTimer = GEM_RESPAWN_TIME;
                timeToNextGem = WAITING_TIME;
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
        playerHasGem = true;
        firstUpdate = true;
    }

    public void lostGem(){
        playerHasGem = false;
        Gem.SetActive(false);
        gemTimer = 0f;
        timeToNextGem = WAITING_TIME / 2;
        firstUpdate = true;
    }

    public void respawn(){
        Gem.SetActive(false);
        gemTimer = 0f;
        timeToNextGem = 0f;
        firstUpdate = true;
    }
}