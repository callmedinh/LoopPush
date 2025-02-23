using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

// Token: 0x02000013 RID: 19
public class PlayerController : MonoBehaviour
{
    // Token: 0x17000012 RID: 18
    // (get) Token: 0x0600005E RID: 94 RVA: 0x00003538 File Offset: 0x00001738
    public static PlayerController Instance
    {
        get
        {
            return PlayerController.instance;
        }
    }

    // Token: 0x0600005F RID: 95 RVA: 0x0000353F File Offset: 0x0000173F
    private void Awake()
    {
        PlayerController.instance = this;
    }

    // Token: 0x17000013 RID: 19
    // (get) Token: 0x06000060 RID: 96 RVA: 0x00003547 File Offset: 0x00001747
    public Point SelfPoint
    {
        get
        {
            return this.point;
        }
    }

    // Token: 0x17000014 RID: 20
    // (get) Token: 0x06000061 RID: 97 RVA: 0x0000354F File Offset: 0x0000174F
    public Point ShadowPoint
    {
        get
        {
            return this.shadowPoint;
        }
    }

    // Token: 0x06000062 RID: 98 RVA: 0x00003558 File Offset: 0x00001758
    public void Init(Vector2Int position, int steps)
    {
        this.shadowPoint = (this.point = MapManager.Instance.GetPoint(position.x, position.y));
        base.transform.position = this.point.Position;
        this.shadow.position = this.point.Position;
        this.stepTotal = steps;
        this.stepLeft = steps;
        this.currentLoopActions = new Direction[steps];
        this.shadow.gameObject.SetActive(this.lastLoopActions != null);
    }

    // Token: 0x06000063 RID: 99 RVA: 0x000035ED File Offset: 0x000017ED
    public void ClearLastLoop()
    {
        this.lastLoopActions = null;
    }

    // Token: 0x06000064 RID: 100 RVA: 0x000035F8 File Offset: 0x000017F8
    public void Move(Direction dir)
    {
        if (this.stepLeft < 1)
        {
            return;
        }
        Point point = null;
        switch (dir)
        {
            case Direction.Left:
                point = MapManager.Instance.GetPoint(this.point.X - 1, this.point.Y);
                this.rendererTransform.localScale = new Vector3(-1f, 1f, 1f);
                break;
            case Direction.Right:
                point = MapManager.Instance.GetPoint(this.point.X + 1, this.point.Y);
                this.rendererTransform.localScale = Vector3.one;
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
        /*
        if (point.isBox)
        {
            if (!BoxManager.Instance.GetBox(point).CanMove(dir))
            {
                return;
            }
            BoxManager.Instance.GetBox(point).Move(dir);
            if (dir == Direction.Left || dir == Direction.Right)
            {
                this.animator.SetTrigger("Push");
            }
        }
        bool flag = false;
        if (point.isTele && !point.TelePoint.isBox)
        {
            point = point.TelePoint;
            flag = true;
        }
        if (point.isKey)
        {
            GameManager.Instance.LevelComplete();
        }
        if (point.isStar)
        {
            MapManager.Instance.DestroyStar();
        }
        */
        this.point = point;
        /*
        if (flag)
        {
            base.transform.DOMove(point.TelePoint.Position, 0.1f, false);
            base.StartCoroutine(this.PlayTeleEffect(point.Position));
        }
        else
        {
            base.transform.DOMove(point.Position, 0.1f, false);
        }*/
        base.transform.DOMove(point.Position, 0.1f, false);
        /*
        MMFeedbacks mmfeedbacks = this.moveFeedback;
        if (mmfeedbacks != null)
        {
            mmfeedbacks.PlayFeedbacks();
        }
        this.currentLoopActions[this.stepTotal - this.stepLeft] = dir;
        if (this.lastLoopActions != null)
        {
            base.StartCoroutine(this.WaitShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]));
        }
        this.StepCost();
        GameManager.Instance.DetectStandKey();
        */
    }


    //private IEnumerator PlayTeleEffect(Vector3 pos)
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    MMFeedbacks mmfeedbacks = this.teleFeedback;
    //    if (mmfeedbacks != null)
    //    {
    //        mmfeedbacks.PlayFeedbacks();
    //    }
    //    base.transform.position = pos;
    //    yield break;
    //}


    private IEnumerator WaitShadowMove(Direction dir)
    {
        yield return new WaitForSeconds(0.1f);
        this.ShadowMove(dir);
        yield break;
    }

    // Token: 0x06000067 RID: 103 RVA: 0x00003858 File Offset: 0x00001A58
    private void ShadowMove(Direction dir)
    {
        Point point = null;
        switch (dir)
        {
            case Direction.Left:
                point = MapManager.Instance.GetPoint(this.shadowPoint.X - 1, this.shadowPoint.Y);
                this.shadowRenderTransform.localScale = new Vector3(-1f, 1f, 1f);
                break;
            case Direction.Right:
                point = MapManager.Instance.GetPoint(this.shadowPoint.X + 1, this.shadowPoint.Y);
                this.shadowRenderTransform.localScale = Vector3.one;
                break;
            case Direction.Up:
                point = MapManager.Instance.GetPoint(this.shadowPoint.X, this.shadowPoint.Y + 1);
                break;
            case Direction.Down:
                point = MapManager.Instance.GetPoint(this.shadowPoint.X, this.shadowPoint.Y - 1);
                break;
        }
        //if (point != null)
        //{
        //    if (point.isBox)
        //    {
        //        if (!BoxManager.Instance.GetBox(point).CanMove(dir))
        //        {
        //            return;
        //        }
        //        BoxManager.Instance.GetBox(point).Move(dir);
        //        if (dir == Direction.Left || dir == Direction.Right)
        //        {
        //            this.animator.SetTrigger("Push");
        //        }
        //    }
        //    if (point.isStar)
        //    {
        //        MapManager.Instance.DestroyStar();
        //    }
        //    bool flag = false;
        //    if (point.isTele && !point.TelePoint.isBox)
        //    {
        //        point = point.TelePoint;
        //        flag = true;
        //    }
        //    Debug.Log(point);
        //    if (point.isKey)
        //    {
        //        this.shadowPoint = point;
        //        GameManager.Instance.LevelComplete();
        //    }
        //    if (flag)
        //    {
        //        this.shadow.DOMove(point.TelePoint.Position, 0.1f, false);
        //        base.StartCoroutine(this.PlayShadowTeleEffect(point.Position));
        //    }
        //    else
        //    {
        //        this.shadow.DOMove(point.Position, 0.1f, false);
        //    }
        //    MMFeedbacks mmfeedbacks = this.shadowMoveFeedback;
        //    if (mmfeedbacks != null)
        //    {
        //        mmfeedbacks.PlayFeedbacks();
        //    }
        //    this.shadowPoint = point;
        //    GameManager.Instance.DetectStandKey();
        //    return;
        //}
        //Object.FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(this.shadow.position));
        //MMFeedbacks mmfeedbacks2 = this.skipFeedback;
        //if (mmfeedbacks2 == null)
        //{
        //    return;
        //}
        //mmfeedbacks2.PlayFeedbacks();
    }


    //private IEnumerator PlayShadowTeleEffect(Vector3 pos)
    //{
    //    yield return new WaitForSeconds(0.1f);
    //    MMFeedbacks mmfeedbacks = this.shadowTeleFeedback;
    //    if (mmfeedbacks != null)
    //    {
    //        mmfeedbacks.PlayFeedbacks();
    //    }
    //    this.shadow.position = pos;
    //    yield break;
    //}

    /*
    public void SkipTurn()
    {
        if (this.lastLoopActions != null)
        {
            this.ShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]);
        }
        this.currentLoopActions[this.stepTotal - this.stepLeft] = Direction.None;
        Object.FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(base.transform.position));
        MMFeedbacks mmfeedbacks = this.skipFeedback;
        if (mmfeedbacks != null)
        {
            mmfeedbacks.PlayFeedbacks();
        }
        this.StepCost();
    }

    // Token: 0x0600006A RID: 106 RVA: 0x00003B17 File Offset: 0x00001D17
    private void StepCost()
    {
        this.stepLeft--;
        GamePanelManager.Instance.StepOneNode();
        if (this.stepLeft == 0)
        {
            base.StartCoroutine(this.WaitEndLoop());
        }
    }

    // Token: 0x0600006B RID: 107 RVA: 0x00003B46 File Offset: 0x00001D46
    private IEnumerator WaitEndLoop()
    {
        yield return new WaitForSeconds(0.5f);
        if (!GameManager.Instance.IsLevelComplete)
        {
            this.OverLoop();
        }
        yield break;
    }

    // Token: 0x0600006C RID: 108 RVA: 0x00003B55 File Offset: 0x00001D55
    private void OverLoop()
    {
        this.lastLoopActions = this.currentLoopActions;
        MapManager.Instance.LoopMapInit();
        CameraController.Instance.PlayReloadFeedback();
        GamePanelManager.Instance.ClearNodeStep();
    }
    */

    private static PlayerController instance;

    [SerializeField]
    private Transform shadow;

    [SerializeField]
    private Transform rendererTransform;

    [SerializeField]
    private Transform shadowRenderTransform;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Animator shadowAnimator;

    /*
    [SerializeField]
    private MMFeedbacks moveFeedback;

    // Token: 0x04000063 RID: 99
    [SerializeField]
    private MMFeedbacks teleFeedback;

    // Token: 0x04000064 RID: 100
    [SerializeField]
    private MMFeedbacks shadowTeleFeedback;

    // Token: 0x04000065 RID: 101
    [SerializeField]
    private MMFeedbacks shadowMoveFeedback;

    // Token: 0x04000066 RID: 102
    [SerializeField]
    private MMFeedbacks skipFeedback;

    */

    // Token: 0x04000067 RID: 103
    private Point point;

    // Token: 0x04000068 RID: 104
    private Point shadowPoint;

    // Token: 0x04000069 RID: 105
    private int stepTotal;

    // Token: 0x0400006A RID: 106
    private int stepLeft;

    // Token: 0x0400006B RID: 107
    private Direction[] currentLoopActions;

    // Token: 0x0400006C RID: 108
    private Direction[] lastLoopActions;
}
