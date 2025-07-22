using UnityEngine;

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }

    public int Y { get; set; }

    public bool IsBox { get; set; }

    public bool IsTele { get; set; }

    public bool IsDst { get; set; }

    public bool IsKey { get; set; }

    public bool IsStar { get; set; }

    public Point TelePoint { get; set; }

    public Vector3 Position { get; set; }

    //operator overloading
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.X + b.X, a.Y + b.Y);
    }
}