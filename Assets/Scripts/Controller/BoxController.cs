using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Point P
    {
        get
        {
            return this.point;
        }
    }

    public bool IsSettled
    {
        get
        {
            return this.isSettled;
        }
    }

    public void Init(Vector2Int position)
    {
        this.point = MapManager.Instance.GetPoint(position.x, position.y);
        transform.position = new Vector3((float)position.x, (float)position.y, -2f);
        this.point.IsBox = true;
        this.isSettled = false;
    }


    public void Move(Direction dir)
    {
        Point point = null;
        switch (dir)
        {
            case Direction.Left:
                point = MapManager.Instance.GetPoint(this.point.X - 1, this.point.Y);
                break;
            case Direction.Right:
                point = MapManager.Instance.GetPoint(this.point.X + 1, this.point.Y);
                break;
            case Direction.Up:
                point = MapManager.Instance.GetPoint(this.point.X, this.point.Y + 1);
                break;
            case Direction.Down:
                point = MapManager.Instance.GetPoint(this.point.X, this.point.Y - 1);
                break;
        }
        if (point == null)
        {
            return;
        }
        bool flag = false;
        if (point.IsTele && !point.TelePoint.IsBox)
        {
            point = point.TelePoint;
            flag = true;
        }
        this.point.IsBox = false;
        if (flag)
        {
            transform.DOMove(point.TelePoint.Position, 0.1f, false);
        }
        else
        {
            transform.DOMove(point.Position + new Vector3(0f, 0f, -1f), 0.1f, false);
        }
        this.point = point;
        this.point.IsBox = true;
        if (!point.IsDst)
        {
            this.isSettled = false;
            return;
        }
        this.isSettled = true;
        BoxManager.Instance.DetectBoxState();
    }
    public bool CanMove(Direction dir)
    {
        Point point = null;
        switch (dir)
        {
            case Direction.Left:
                point = MapManager.Instance.GetPoint(this.point.X - 1, this.point.Y);
                break;
            case Direction.Right:
                point = MapManager.Instance.GetPoint(this.point.X + 1, this.point.Y);
                break;
            case Direction.Up:
                point = MapManager.Instance.GetPoint(this.point.X, this.point.Y + 1);
                break;
            case Direction.Down:
                point = MapManager.Instance.GetPoint(this.point.X, this.point.Y - 1);
                break;
        }
        return point != null;
    }

    private Point point;
    private bool isSettled;
}
