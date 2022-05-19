using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static const int N_TURNS = 6;

    private IOManager ioManager;

    public GameManager() {
        this.ioManager = new IOManager();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    void CreateNewGame()
    {
        CurlingGame game = new CurlingGame(N_TURNS);
        ioManager.SetGame(game);
    }
}
