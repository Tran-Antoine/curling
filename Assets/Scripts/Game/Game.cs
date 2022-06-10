public interface Game
{

    void PlayTurn();

    void PlayThrow(StaticStone stone);

    bool ExpectsThrow();

    void StartGame();
    void EndGame();
    void EndTurn();
    void EndThrow();
    void MarkReadyForThrow();
    void ExpectedWasThrown();

    int ActivePlayer();
    GameState GetState();
}