using System.Collections.Generic;
using System.Numerics;

class ScoreCalculator {

    public static (int, int) ComputeScore(List<StaticStone> stones)
    {
        stones.Sort(CompareByDistance);

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
        float d1 = Vector3.DistanceSquared(GameState.CENTER, s1.GetPosition());
        float d2 = Vector3.DistanceSquared(GameState.CENTER, s2.GetPosition());

        return d1 - d2;
    }
}