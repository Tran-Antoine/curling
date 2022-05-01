using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class musicManagerGame : MonoBehaviour
{  
    // Start is called before the first frame update
    AudioSource audio;
    private bool paused;
    public Toggle musicToggle;
    public Slider slider;

    void Start()
    {   BGSoundScript[] musics = Object.FindObjectsOfType<BGSoundScript>();
        if (musics.Length >= 1) {
        BGSoundScript music = Object.FindObjectsOfType<BGSoundScript>()[0];
        audio = music.GetComponent<AudioSource>();
        if(!audio.enabled) {
            audio.enabled = true;
            audio.Pause();
        }
        paused = !audio.isPlaying;
        musicToggle.isOn = audio.isPlaying;
        slider.value = audio.volume;
        } else {
            slider.value = 0;
            audio = null;
        }
    }

    void Update(){
        audio.volume = slider.value;
    }

    public void PausePlay(){
        if (!musicToggle.isOn){
             audio.Pause();
             paused = true;
        }
        if(musicToggle.isOn){
            audio.Play(0);
            paused = false;
        }
    }
}
