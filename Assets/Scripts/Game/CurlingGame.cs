public class CurlingGame : Game
{

    private GameState state;

    public CurlingGame(int turns)
    {
        this.state = new GameState(turns);
    }

    
    public void PlayTurn()
    {
        if(state.GetRemainingTurns == 0)
        {
            OnGameEnded();
            return;
        }

        do {

            int activePlayer = state.GetActivePlayer();

            StaticStone resultingStone = (...);
            List<int> impactedStones = (...);
            List<Vector3> newPositions = (...);

            state.AddStone(resultingStone);
            for(int i = 0; i < impactedStones.Capacity; i++)
            {
                state.ChangeStone(impactedStones.ElementAt(i), newPositions.ElementAt(i));
            }
            
            state.SwitchActivePlayer();
            OnThrowEnded();

        } while (!state.IsNewTurnStarting());
    }

    public void OnGameStarted()
    {

    }
    
    public void OnGameEnded()
    {

    }
    
    public void OnTurnEnded()
    {

    }

    public void OnThrowEnded()
    {

    }
}
