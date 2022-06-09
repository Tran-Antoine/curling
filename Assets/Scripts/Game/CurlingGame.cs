using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public class CurlingGame : Game
{

    private GameState state;
    private IOManager manager;

    public CurlingGame(int turns, IOManager manager)
    {
        this.state = new GameState(turns);
        this.manager = manager;
    }

    
    public void PlayTurn()
    {
        if(!state.IsWaitingForNext()) {
            return;
        }

        if(state.GetRemainingTurns() == 0)
        {
            manager.OnGameEnded();
            return;
        }
    }

    public void PlayThrow(StaticStone stone)
    {
        state.AddStone(stone);
        state.SetWaitingForNext(false);
        int activePlayer = state.GetActivePlayer();
        stone.SetPlayer(activePlayer);
            
        state.SwitchActivePlayer();

        if(state.IsNewTurnStarting()) {
            if (state.GetRemainingTurns() == 0) {
                //PrepareForNextTurn();
                EndGame();
                return;
            } else {
                EndThrow();
                EndTurn();
                manager.onEndTurn();
                return;
            }
        }
        EndThrow();
    }

    public bool ExpectsThrow()
    {
        return state.IsWaitingForNext();
    }

    public void StartGame()
    {
        manager.OnGameStarted();
        state.SetWaitingForNext(true);
        PlayTurn();
    }
    
    public void EndGame()
    {
        (int score, int scorer) = ScoreCalculator.ComputeScore(state.GetStones());
        manager.ShowScore(score, scorer);
        manager.OnGameEnded();
    }
    
    public void EndTurn()
    {
        //PrepareForNextTurn();
        PlayTurn();
    }

    public void PrepareForNextTurn(){
        //(int score, int scorer) = ScoreCalculator.ComputeScore(state.GetStones());
        //manager.ShowScore(score, scorer);
    }

    public void EndThrow()
    {
        manager.AllowNext();
    }

    public void MarkReadyForThrow() {
        this.state.SetWaitingForNext(true);
    }

    public void ExpectedWasThrown(){
        state.SetWaitingForNext(false);
    }

    public int GetCurrPlayer()
    {
        return state.GetActivePlayer();
    }
}
