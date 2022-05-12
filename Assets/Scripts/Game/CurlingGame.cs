public class CurlingGame : Game
{

    private GameState state;
    private IOManager manager;

    public CurlingGame(int turns)
    {
        this.state = new GameState(turns);
    }

    public void SetIO(IOManager manager)
    {
        this.manager = manager;
    }

    
    public void PlayTurn()
    {

        if(!state.IsWaitingForNext()) {
            return;
        }

        if(state.GetRemainingTurns == 0)
        {
            OnGameEnded();
            return;
        }
    }

    public void PlayThrow(Trajectory traj)
    {

        state.SetWaitingForNext(false);
        int activePlayer = state.GetActivePlayer();

        StaticStone resultingStone = (...); // some code that depends on traj
        List<int> impactedStones = (...);
        List<Vector3> newPositions = (...);

        state.AddStone(resultingStone);
        for(int i = 0; i < impactedStones.Capacity; i++)
        {
            state.ChangeStone(impactedStones.ElementAt(i), newPositions.ElementAt(i));
        }
            
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
        state.SetWaitingForNext(true);
        PlayTurn();
    }
    
    public void EndGame()
    {

    }
    
    public void EndTurn()
    {

    }

    public void EndThrow()
    {

    }

    public void MarkReadyForThrow() {
        this.state.SetWaitingForNext(true);
    }
}
