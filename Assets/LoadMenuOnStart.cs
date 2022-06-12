using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMenuOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!MenuAlreadyLoadedBool.alreadyDone){
            SceneManager.LoadScene("Menu");
            MenuAlreadyLoadedBool.alreadyDone = true;
        }
    }

}