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

    public Button continueGame;
    public GameObject gameOverBox;
    public ToggleGroupTurns toggleGroupOptions;
    public TextMeshProUGUI startGameText;
    public TextMeshProUGUI niceThrow;
    public List<string> messages = new List<string>(){"Nice job!", "Well done!", "Wow!", "Great!", "Impressive!", "Keep it up!", "Awesome!", "Amazing!", "Perfect!", "Phenomenal!", "Meh...", "Better luck next time"};
    
    private int turnsLeft;
    public TextMeshProUGUI turnsText;
    private int currPlayer;
    public TextMeshProUGUI currPlayerText;
    
    public TextMeshProUGUI winnerText;


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
        turnsLeft = toggleGroupOptions.turns;
        SetGame(new CurlingGame(turnsLeft, this));
        if(game == null) return;
        turnsText.text = "Turns left : " + (turnsLeft).ToString();
        game.StartGame();
        currPlayer = game.GetCurrPlayer();
    }

    /// Triggered by Unity
    public void onNextClicked() 
    {   currPlayer = game.GetCurrPlayer();
        currPlayerText.text = "Player : " + (currPlayer+1).ToString();
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
        turnsText.text = "Turns left : 0";
        StartCoroutine(WaitForSec(niceThrow));
        StartCoroutine(WaitForSec());
    }
    

    private IEnumerator RegisterVelocity(SimulationStone stone){
            yield return new WaitForSeconds(0.4f);
            stone.RegisterAngle();
    }

    /**
    private IEnumerator RegisterAngle(SimulationStone stone){
            yield return new WaitForSeconds(0.5f);
            stone.ThrowStoneFromCurrentVelocities();
    }*/
    
    private IEnumerator LOL(SimulationStone stone){
            yield return new WaitForSeconds(0.55f);
            stone.ThrowStoneFromCurrentVelocities();
    }



    /// Triggered by Unity
    /// TODO: Connecter l'??v??nement "Le robot passe la ligne de d??part et donc est consid??r?? comme lanc??" ?? cette m??thode (dans le code li?? ?? Unity probablement)
    public void OnStoneThrown(SimulationStone stone) 
    {
        if(game == null) return;
        
        if(game.ExpectsThrow()) // if this returns false, it means that the stone was most likely thrown by accident
        {   
            this.pendingData = stone;

            stone.RegisterPosition();
            
            StartCoroutine(RegisterVelocity(stone));
            StartCoroutine(LOL(stone));
            CelluloVisualisation.SetColor(stone.cellulo, game.ActivePlayer());

            stone.SetThrown(true);
            ++throws;

            game.ExpectedWasThrown();
        }
    }

    /// Triggered by Unity / The Simulation manager
    /// TODO: Connecter l'??v??nement "La simulation du lancer est termin??e" ?? cette m??thode
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
    {   if (score == 0) {
            winnerText.text = "It's a tie! You both suck equally!";
    } else {
        winnerText.text = "Player " + (scorer+1).ToString() + " wins with " + score.ToString() + " points!";

    }
    }

    public void onEndTurn()
    { 
        turnsText.text = "Turns left : " + (--turnsLeft).ToString();
    }

    /// Triggered by Game
    public void AllowNext()
    {
        continueGame.gameObject.SetActive(true);
    }

    public void OnSwapRequested(SimulationStone s1, SimulationStone s2)
    {
        StaticStone data1 = s1.GetLogicStone();
        StaticStone data2 = s2.GetLogicStone();

        s1.SetLogicStone(data2);
        s2.SetLogicStone(data1);
    }
}