using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
/// Bridge that connects the game logic and the unity simulation
public class IOManager : MonoBehaviour
{

    private Game game;

    private SimulationStone pendingData;


    public TextMeshProUGUI scoreTextP0;
    public TextMeshProUGUI scoreTextP1;
    public Button continueGame;
    public GameObject gameOverBox;
    public int turns;
    public ToggleGroupTurns toggleGroupOptions;
    public TextMeshProUGUI startGameText;
    public TextMeshProUGUI niceThrow;
    public List<string> messages = new List<string>(){"Nice job!", "Well done!", "Wow!", "Great!", "Impressive!", "Keep it up!", "Awesome!", "Amazing!", "Perfect!", "Phenomenal!", "Meh...", "Better luck next time"};

    private int throws = 0;
    public void SetGame(Game game) 
    {
        this.game = game;
    }

    public Game GetGame()
    {
        return game;
    }

    /// Triggered by Unity
    public void onStartClicked() 
    {   
        toggleGroupOptions.Choose();
        while(toggleGroupOptions.turns == 0){}
        SetGame(new CurlingGame(toggleGroupOptions.turns, this));
        if(game == null) return;

        game.StartGame();
    }

    /// Triggered by Unity
    public void onNextClicked() 
    {
        if(game == null) return;

        game.MarkReadyForThrow();
    }

    private IEnumerator WaitForSec(TextMeshProUGUI text){
            yield return new WaitForSeconds(1);
            text.gameObject.SetActive(false);
    }

    private IEnumerator WaitForSec(){
            yield return new WaitForSeconds(2);
            
            gameOverBox.SetActive(true);
    }

    private IEnumerator RecallClosestWithDelay(){
            yield return new WaitForSeconds(3);
            CelluloVisualisation.RecallClosestCellulo();
    }

    /// Triggered by Game
    public void OnGameStarted() 
    {   
        startGameText.gameObject.SetActive(true);
        StartCoroutine(WaitForSec(startGameText));
    }

    /// Triggered by Game
    public void OnGameEnded() 
    {   
        StartCoroutine(WaitForSec(niceThrow));
        StartCoroutine(WaitForSec());
    }

    /// Triggered by Unity
    /// TODO: Connecter l'évènement "Le robot passe la ligne de départ et donc est considéré comme lancé" à cette méthode (dans le code lié à Unity probablement)
    public void OnStoneThrown(SimulationStone stone) 
    {
        if(game == null) return;
        
        if(game.ExpectsThrow()) // if this returns false, it means that the stone was most likely thrown by accident
        {   
            this.pendingData = stone;

            stone.ThrowStoneFromCurrentVelocities();
            //stone.ThrowStone(new Vector3(0.5f, 0f, 0f), stone.getPosition(), -0.40f);

            stone.SetThrown(true);
            ++throws;

            game.ExpectedWasThrown();
        }
    }

    /// Triggered by Unity / The Simulation manager
    /// TODO: Connecter l'évènement "La simulation du lancer est terminée" à cette méthode
    public void OnThrowSimulationEnded()
    {  
        if(game == null || pendingData == null) return; // pendingData should normally never be null at this stage

        if (throws > 2){
            StartCoroutine(RecallClosestWithDelay());
        }
        game.PlayThrow(pendingData.GetLogicStone());
        pendingData = null;
        
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, messages.Count);
        niceThrow.text = messages[num];
        niceThrow.gameObject.SetActive(true);
    }

    /// Triggered by Game
    public void ShowScore(int score, int scorer)
    {
        if (scorer == 0) {
            scoreTextP0.text = score.ToString();
        } else {
            scoreTextP1.text = score.ToString();
        }
    }

    /// Triggered by Game
    public void AllowNext()
    {
        continueGame.gameObject.SetActive(true);
    }
}