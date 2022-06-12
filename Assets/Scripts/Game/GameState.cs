using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameState
{

    private const int N_PLAYERS = 2;
    public static Vector3 CENTER = new Vector3(11.63f, 0, -4.95f);

    public static float RADIUS = 4.22f;
    private bool waitingForNext;
    private int turns;
    private int remainingTurns; // number of turns remaining 
    private int activePlayer; // 0 => first player, 1 => second player

    private int nStoneThrown = 0;

    private List<StaticStone> stones;

    public GameState(int turns)
    {
        this.turns = turns;
        this.remainingTurns = turns;
        this.activePlayer = 0;
        this.stones = new List<StaticStone>(turns * N_PLAYERS + 3);
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
        stone.SetId(nStoneThrown++);
    }


    public int GetClosestPlayer(Vector3 position)
    {
        float dis = 1000000;
        StaticStone closest = null;

        foreach (StaticStone stone in stones)
        {
            float stoneDis = Vector3.Distance(stone.GetPosition(), position);

            if(stoneDis < dis)
            {
                dis = stoneDis;
                closest = stone;
            }
        }

        return closest.GetPlayer();
    }
}