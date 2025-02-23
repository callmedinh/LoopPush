using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// Token: 0x02000002 RID: 2
public class BoxController : MonoBehaviour
{
    // Token: 0x17000001 RID: 1
    // (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
    public Point P
    {
        get
        {
            return this.point;
        }
    }

    // Token: 0x17000002 RID: 2
    // (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
    public bool IsSettled
    {
        get
        {
            return this.isSettled;
        }
    }

    // Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
    public void Init(Vector2Int position)
    {
        this.point = MapManager.Instance.GetPoint(position.x, position.y);
        base.transform.position = new Vector3((float)position.x, (float)position.y, 0f);
        this.point.isBox = true;
        this.isSettled = false;
    }

    /*
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
        if (point.isTele && !point.TelePoint.isBox)
        {
            point = point.TelePoint;
            flag = true;
        }
        this.point.isBox = false;
        if (flag)
        {
            base.transform.DOMove(point.TelePoint.Position, 0.1f, false);
            base.StartCoroutine(this.PlayTeleEffect(point.Position));
        }
        else
        {
            base.transform.DOMove(point.Position, 0.1f, false);
        }
        this.point = point;
        this.point.isBox = true;
        if (!point.isDst)
        {
            this.isSettled = false;
            return;
        }
        this.isSettled = true;
        BoxManager.Instance.DetectBoxState();
        MMFeedbacks mmfeedbacks = this.settleFeedback;
        if (mmfeedbacks == null)
        {
            return;
        }
        mmfeedbacks.PlayFeedbacks();
    }

    // Token: 0x06000005 RID: 5 RVA: 0x00002241 File Offset: 0x00000441
    private IEnumerator PlayTeleEffect(Vector3 pos)
    {
        yield return new WaitForSeconds(0.1f);
        MMFeedbacks mmfeedbacks = this.teleFeedback;
        if (mmfeedbacks != null)
        {
            mmfeedbacks.PlayFeedbacks();
        }
        base.transform.position = pos;
        yield break;
    }

    // Token: 0x06000006 RID: 6 RVA: 0x00002258 File Offset: 0x00000458
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
    */

    // Token: 0x04000001 RID: 1
    private Point point;

    // Token: 0x04000002 RID: 2
    private bool isSettled;

    /*
    [SerializeField]
    private MMFeedbacks teleFeedback;

    // Token: 0x04000004 RID: 4
    [SerializeField]
    private MMFeedbacks settleFeedback;
    */
}
