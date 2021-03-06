using System.Collections.Generic;
using UnityEngine;

class ScoreCalculator {

    private static Vector3 ABSOLUTE_CENTER = new Vector3(25.9f, -1.5f, 0.1f);
    public static (int, int) ComputeScore(List<StaticStone> stones)
    {
        stones = stones.FindAll(x => Vector3.Distance(x.GetPosition(), ABSOLUTE_CENTER) < GameState.RADIUS);

        stones.Sort(CompareByDistance);

        if (stones.Count == 0){
            return(0, 0);
        }

        int scoringPlayer = stones[0].GetPlayer();

        int score = 0;

        foreach (var stone in stones)
        {
            if(stone.GetPlayer() == scoringPlayer)
            {
                score += 1;
            } 
            else 
            {
                break;
            }
        }

        return (score, scoringPlayer);
    }

    private static int CompareByDistance(StaticStone s1, StaticStone s2)
    {
        float d1 = Vector3.Distance(ABSOLUTE_CENTER, s1.GetPosition());
        float d2 = Vector3.Distance(ABSOLUTE_CENTER, s2.GetPosition());

        return d1 - d2 < 0 ? -1 : 1;
    }
}