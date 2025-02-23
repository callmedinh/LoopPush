using UnityEngine;

public class Point : MonoBehaviour
{
    public int X { get; set; }

    public int Y { get; set; }

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public bool isBox { get; set; }

    public bool isTele { get; set; }

    public bool isDst { get; set; }

    public bool isKey { get; set; }

    public bool isStar { get; set; }

    public Point TelePoint { get; set; }

    public Vector3 Position { get; set; }
}
