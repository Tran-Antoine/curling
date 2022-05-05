using UnityEngine;

public class StaticStone
{
    private Vector3 position;
    private const int player;
    private const int id;

    public StaticStone(Vector3 position, int player, int id)
    {
        this.position = position;
        this.player = player;
        this.id = id;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public int GetPlayer()
    {
        return player;
    }

    public int GetId()
    {
        return id;
    }

    public void SetPosition(Vector3 newPosition)
    {
        this.position = newPosition;
    }
}