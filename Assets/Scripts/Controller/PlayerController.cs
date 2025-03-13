using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance
    {
        get
        {
            return PlayerController.instance;
        }
    }

    private void Awake()
    {
        PlayerController.instance = this;
    }

    public Point SelfPoint
    {
        get
        {
            return this.point;
        }
    }

    public Point ShadowPoint
    {
        get
        {
            return this.shadowPoint;
        }
    }

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

    public void ClearLastLoop()
    {
        this.lastLoopActions = null;
    }

    public void Move(Direction dir)
    {
        if (this.stepLeft < 1)
        {
            Debug.Log(stepLeft);
            return;
        }
        Point _point = null;
        switch (dir)
        {
            case Direction.Left:
                _point = MapManager.Instance.GetPoint(this.point.X - 1, this.point.Y);
                this.rendererTransform.localScale = new Vector3(-1f, 1f, 1f);
                break;
            case Direction.Right:
                _point = MapManager.Instance.GetPoint(this.point.X + 1, this.point.Y);
                this.rendererTransform.localScale = Vector3.one;
                break;
            case Direction.Up:
                _point = MapManager.Instance.GetPoint(this.point.X, this.point.Y + 1);
                break;
            case Direction.Down:
                _point = MapManager.Instance.GetPoint(this.point.X, this.point.Y - 1);
                break;
        }
        if (_point == null)
        {
            Debug.Log($"Trying to move to: ({this.point.X}, {this.point.Y}) - Got point: {_point}");
            return;
        }
        
        if (_point.isBox)
        {
            if (!BoxManager.Instance.GetBox(_point).CanMove(dir))
            {
                return;
            }
            BoxManager.Instance.GetBox(_point).Move(dir);
            //if (dir == Direction.Left || dir == Direction.Right)
            //{
            //    this.animator.SetTrigger("Push");
            //}
        }
        bool flag = false;
        if (_point.isTele && !_point.TelePoint.isBox)
        {
            _point = _point.TelePoint;
            flag = true;
        }
        //if (_point.isKey)
        //{
        //    GameManager.Instance.LevelComplete();
        //}
        //if (_point.isStar)
        //{
        //    MapManager.Instance.DestroyStar();
        //}
        this.point = _point;
        if (flag)
        {
            base.transform.DOMove(point.TelePoint.Position, 0.1f, false);
            //base.StartCoroutine(this.PlayTeleEffect(point.Position));
        }
        else
        {
            base.transform.DOMove(point.Position, 0.1f, false);
        }
        base.transform.DOMove(_point.Position, 0.1f, false);
        
        //MMFeedbacks mmfeedbacks = this.moveFeedback;
        //if (mmfeedbacks != null)
        //{
        //    mmfeedbacks.PlayFeedbacks();
        //}
        this.currentLoopActions[this.stepTotal - this.stepLeft] = dir;
        if (this.lastLoopActions != null)
        {
            base.StartCoroutine(this.WaitShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]));
        }
        this.StepCost();
        //GameManager.Instance.DetectStandKey();
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

    private void ShadowMove(Direction dir)
    {
        Point _point = null;
        switch (dir)
        {
            case Direction.Left:
                _point = MapManager.Instance.GetPoint(this.shadowPoint.X - 1, this.shadowPoint.Y);
                this.shadowRenderTransform.localScale = new Vector3(-1f, 1f, 1f);
                break;
            case Direction.Right:
                _point = MapManager.Instance.GetPoint(this.shadowPoint.X + 1, this.shadowPoint.Y);
                this.shadowRenderTransform.localScale = Vector3.one;
                break;
            case Direction.Up:
                _point = MapManager.Instance.GetPoint(this.shadowPoint.X, this.shadowPoint.Y + 1);
                break;
            case Direction.Down:
                _point = MapManager.Instance.GetPoint(this.shadowPoint.X, this.shadowPoint.Y - 1);
                break;
        }
        if (_point != null)
        {
            if (_point.isBox)
            {
                if (!BoxManager.Instance.GetBox(_point).CanMove(dir))
                {
                    return;
                }
                BoxManager.Instance.GetBox(_point).Move(dir);
                //if (dir == Direction.Left || dir == Direction.Right)
                //{
                //    this.animator.SetTrigger("Push");
                //}
            }
            if (_point.isStar)
            {
                MapManager.Instance.DestroyStar();
            }
            bool flag = false;
            if (_point.isTele && !_point.TelePoint.isBox)
            {
                _point = _point.TelePoint;
                flag = true;
            }
            Debug.Log(point);
            if (_point.isKey)
            {
                this.shadowPoint = _point;
                //GameManager.Instance.LevelComplete();
            }
            if (flag)
            {
                this.shadow.DOMove(_point.TelePoint.Position, 0.1f, false);
                //base.StartCoroutine(this.PlayShadowTeleEffect(point.Position));
            }
            else
            {
                this.shadow.DOMove(_point.Position, 0.1f, false);
            }
            //MMFeedbacks mmfeedbacks = this.shadowMoveFeedback;
            //if (mmfeedbacks != null)
            //{
            //    mmfeedbacks.PlayFeedbacks();
            //}
            this.shadowPoint = _point;
            //GameManager.Instance.DetectStandKey();
            return;
        }
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

    public void SkipTurn()
    {
        if (this.lastLoopActions != null)
        {
            this.ShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]);
        }
        this.currentLoopActions[this.stepTotal - this.stepLeft] = Direction.None;
        //Object.FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(base.transform.position));
        //MMFeedbacks mmfeedbacks = this.skipFeedback;
        //if (mmfeedbacks != null)
        //{
        //    mmfeedbacks.PlayFeedbacks();
        //}
        this.StepCost();
    }

    private void StepCost()
    {
        this.stepLeft--;
        GamePanelManager.Instance.StepOneNode();
        if (this.stepLeft == 0)
        {
            base.StartCoroutine(this.WaitEndLoop());
        }
    }

    private IEnumerator WaitEndLoop()
    {
        yield return new WaitForSeconds(0.5f);
        if (!GameManager.Instance.IsLevelComplete)
        {
            this.OverLoop();
        }
        yield break;
    }

    private void OverLoop()
    {
        this.lastLoopActions = this.currentLoopActions;
        Debug.Log(lastLoopActions);
        MapManager.Instance.LoopMapInit();
        //CameraController.Instance.PlayReloadFeedback();
        //GamePanelManager.Instance.ClearNodeStep();
    }

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

    [SerializeField]
    private MMFeedbacks teleFeedback;

    [SerializeField]
    private MMFeedbacks shadowTeleFeedback;

    [SerializeField]
    private MMFeedbacks shadowMoveFeedback;

    [SerializeField]
    private MMFeedbacks skipFeedback;

    */

    private Point point;

    private Point shadowPoint;

    private int stepTotal;

    private int stepLeft;

    private Direction[] currentLoopActions;

    private Direction[] lastLoopActions;
}
