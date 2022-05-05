using System.Collections.Generic;

public class GameState
{

    private static const int N_PLAYERS = 2;

    private int turns;
    private int remainingTurns; // number of turns remaining 
    private int activePlayer; // 0 => first player, 1 => second player

    private int[] scores;

    private List<StaticStone> stones;

    public GameState(int turns)
    {
        this.turns = turns;
        this.remainingTurns = turns;
        this.activePlayer = 0;
        this.scores = new int[N_PLAYERS];
        this.stones = new List<StaticStone>(turns * N_PLAYERS);
    }

    public int GetRemainingTurns()
    {
        return remainingTurns;
    }

    public int GetActivePlayer()
    {
        return activePlayer;
    }

    public int[] GetScores()
    {
        return scores;
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