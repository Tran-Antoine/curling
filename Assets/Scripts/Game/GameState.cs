using System.Collections.Generic;

public class GameState
{

    private static const int N_PLAYERS = 2;
    public static const Vector3 CENTER = new Vector3(0, 0, 0);
    private boolean waitingForNext;
    private int turns;
    private int remainingTurns; // number of turns remaining 
    private int activePlayer; // 0 => first player, 1 => second player

    private List<StaticStone> stones;

    public GameState(int turns)
    {
        this.turns = turns;
        this.remainingTurns = turns;
        this.activePlayer = 0;
        this.scores = new int[N_PLAYERS];
        this.stones = new List<StaticStone>(turns * N_PLAYERS);
        this.waitingForNext = false;
    }

    public bool IsWaitingForNext() {
        return waitingForNext;
    }

    public void SetWaitingForNext(bool val) {
        this.waitingForNext = val;
    }
    
    public int GetRemainingTurns()
    {
        return remainingTurns;
    }

    public int GetActivePlayer()
    {
        return activePlayer;
    }

    public List<StaticStone> GetStones()
    {
        return stones;
    }

    public void SwitchActivePlayer()
    {
        activePlayer = (activePlayer + 1) % N_PLAYERS;
        if(IsNewTurnStarting())
        {
            remainingTurns--;
        }
    }

    public bool IsNewTurnStarting()
    {
        return activePlayer == 0;
    }

    public void AddStone(StaticStone stone)
    {
        stones.Add(stone);
    }

    public void ImpactStone(int id, Vector3 newPosition)
    {
        foreach (StaticStone stone in stones)
        {
            if(stone.GetId() == id)
            {
                stone.SetPosition(newPosition);
                return;
            }
        }
    }
}