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

        state.SetWaitingForNext(false);

        if(state.GetRemainingTurns() == 0)
        {
            manager.OnGameEnded();
            return;
        }
    }

    public void PlayThrow(Trajectory traj)
    {

        /*state.SetWaitingForNext(false);
        int activePlayer = state.GetActivePlayer();

        StaticStone resultingStone = (...); // some code that depends on traj
        List<int> impactedStones = (...);
        List<Vector3> newPositions = (...);

        state.AddStone(resultingStone);
        for(int i = 0; i < impactedStones.Capacity; i++)
        {
            state.ChangeStone(impactedStones[i], newPositions[i]);
        }
            
        state.SwitchActivePlayer();
        EndThrow();

        if(state.IsNewTurnStarting()) {
            EndTurn();
        }*/
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
        manager.OnGameEnded();
    }
    
    public void EndTurn()
    {
        (int score, int scorer) = ScoreCalculator.ComputeScore(state.GetStones());
        manager.ShowScore(score, scorer);
    }

    public void EndThrow()
    {
        manager.AllowNext();
    }

    public void MarkReadyForThrow() {
        this.state.SetWaitingForNext(true);
    }
}
