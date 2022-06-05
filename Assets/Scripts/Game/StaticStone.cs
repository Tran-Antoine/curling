using UnityEngine;

public class StaticStone
{
    private Vector3 position;
    private int player;
    private int id;

    public StaticStone(Vector3 position)
    {
        this.position = position;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public int GetPlayer()
    {
        return player;
    }

    public void SetPlayer(int player)
    {
        this.player = player;
    }

    public void SetId(int id)
    {
        this.id = id;
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