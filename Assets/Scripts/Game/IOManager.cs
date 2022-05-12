/// Bridge that connects the game logic and the unity simulation
public class IOManager 
{

    private const Game game;

    private Trajectory pendingData;

    public void SetGame(Game game) 
    {
        this.game = game;
    }

    /// Triggered by Unity
    public void onStartClicked() 
    {
        if(game == null) return;

        game.StartGame()
    }

    /// Triggered by Unity
    public void onNextClicked() 
    {
        if(game == null) return;

        game.MarkReadyForThrow();
    }

    /// Triggered by Game
    public void onGameStarted() 
    {

    }

    /// Triggered by Game
    public void onGameEnded() 
    {

    }

    /// Triggered by Unity
    public void onStoneThrown(Trajectory traj) 
    {
        if(game == null) return;

        if(game.ExpectsThrow())
        {
            this.pendingData = traj;
            PlaySimulation();
        }
    }

    /// Triggered by Unity / The Simulation manager
    public void onThrowSimulationEnded()
    {
        if(game == null || pendingData == null) return;

        game.PlayThrow(pendingData);
        pendingData = null;
    }

    private void PlaySimulation()
    {
        // TODO : add code that makes the robot follow the trajectory step by step
    }
}