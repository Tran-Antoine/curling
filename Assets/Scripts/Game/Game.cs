public interface Game
{

    void PlayTurn();

    void OnGameStarted();
    void OnGameEnded();
    void OnTurnEnded();
    void OnThrowEnded();
}