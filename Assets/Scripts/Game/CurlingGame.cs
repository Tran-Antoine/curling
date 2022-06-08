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
        Console.WriteLine("Playturn !");

        if(!state.IsWaitingForNext()) {
            return;
        }

        state.SetWaitingForNext(false);

        if(state.GetRemainingTurns() == 0)
        {
            manager.OnGameEnded();
            return;
        }
    }

    public void PlayThrow(StaticStone stone)
    {
        Console.WriteLine("PlayThrow' !");
        state.SetWaitingForNext(false);
        int activePlayer = state.GetActivePlayer();
        stone.SetPlayer(activePlayer);
            
        state.SwitchActivePlayer();
        EndThrow();

        if(state.IsNewTurnStarting()) {
            EndTurn();
        }
    }

    public bool ExpectsThrow()
    {
        return state.IsWaitingForNext();
    }

    public void StartGame()
    {
        Console.WriteLine("StartGame !");
        manager.OnGameStarted();
        state.SetWaitingForNext(true);
        PlayTurn();
    }
    
    public void EndGame()
    {
        Console.WriteLine("Endgame !");
        manager.OnGameEnded();
    }
    
    public void EndTurn()
    {
        Console.WriteLine("End Turn called");
        (int score, int scorer) = ScoreCalculator.ComputeScore(state.GetStones());
        manager.ShowScore(score, scorer);
    }

    public void EndThrow()
    {
        Console.WriteLine("EndThrow !");
        manager.AllowNext();
    }

    public void MarkReadyForThrow() {
        Console.WriteLine("MarkReadyForThrow() !");
        this.state.SetWaitingForNext(true);
    }
}
