public interface Game
{

    void playTurn();

    void onGameStarted();
    void onGameEnded();
    void onTurnEnded();
    void onThrowEnded();
}