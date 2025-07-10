using UnityEngine;

public class Point
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool IsBox { get; set; }

    public bool IsTele { get; set; }

    public bool IsDst { get; set; }

    public bool IsKey { get; set; }

    public bool IsStar { get; set; }

    public Point TelePoint { get; set; }

    public Vector3 Position { get; set; }
}
