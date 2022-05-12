public interface Game
{

    void PlayTurn();

    void StartGame();
    void EndGame();
    void EndTurn();
    void EndThrow();
    void MarkReadyForThrow();
}