public interface Game
{

    void PlayTurn();

    void PlayThrow(Trajectory traj);

    bool ExpectsThrow();

    void StartGame();
    void EndGame();
    void EndTurn();
    void EndThrow();
    void MarkReadyForThrow();
}