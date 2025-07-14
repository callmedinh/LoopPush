using System.Collections;
using Audio;
using DG.Tweening;
using Events;
using Manager;
using UnityEngine;
using Utilities;

namespace Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        private static readonly int Slide = Animator.StringToHash("Slide");

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

        protected override void Awake()
        {
            base.Awake();
            InputEvent.OnDownDirectionPressed += (() => Move(Direction.Down));
            InputEvent.OnUpDirectionPressed += (() => Move(Direction.Up));
            InputEvent.OnLeftDirectionPressed += (() => Move(Direction.Left));
            InputEvent.OnRightDirectionPressed += (() => Move(Direction.Right));
        }

        public void Init(Vector2Int position, int steps)
        {
            this.shadowPoint = (this.point = MapManager.Instance.GetPoint(position.x, position.y));
            transform.position = this.point.Position + new Vector3(0f,0f,-1f);
            this.shadow.position = this.point.Position + new Vector3(0f, 0f, -1f);
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

            if (_point != null)
            {
                if (_point.IsBox)
                {
                    if (!BoxManager.Instance.GetBox(_point).CanMove(dir))
                    {
                        return;
                    }
                    BoxManager.Instance.GetBox(_point).Move(dir);
                    if (dir == Direction.Left || dir == Direction.Right)
                    {
                        animator.SetTrigger(Slide);
                    }
                }
                bool flag = false;
                if (_point.IsTele && !_point.TelePoint.IsBox)
                {
                    _point = _point.TelePoint;
                    flag = true;
                }
                this.point = _point;
                if (flag)
                {
                    transform.DOMove(point.TelePoint.Position, 0.1f);
                }
                else
                {
                    transform.DOMove(point.Position, 0.1f);
                }
                transform.DOMove(_point.Position + new Vector3(0,0,-1f), 0.1f);
        
                currentLoopActions[this.stepTotal - this.stepLeft] = dir;
                if (this.lastLoopActions != null)
                {
                    StartCoroutine(this.WaitShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]));
                }
                StepCost();
            }
        }

        private IEnumerator WaitShadowMove(Direction dir)
        {
            yield return new WaitForSeconds(0.1f);
            ShadowMove(dir);
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
                if (_point.IsBox)
                {
                    if (!BoxManager.Instance.GetBox(_point).CanMove(dir))
                        return;

                    BoxManager.Instance.GetBox(_point).Move(dir);
                    if (dir == Direction.Left || dir == Direction.Right)
                        animator.SetTrigger(Slide);
                }

                if (_point.IsStar)
                {
                    MapManager.Instance.DestroyStar();
                }

                if (_point.IsTele && !_point.TelePoint.IsBox)
                {
                    _point = _point.TelePoint;
                }

                this.shadow.DOMove(_point.Position + new Vector3(0f, 0f, -1f), 0.1f);
                this.shadowPoint = _point;
            }
        }


        public void SkipTurn()
        {
            if (this.lastLoopActions != null)
            {
                this.ShadowMove(this.lastLoopActions[this.stepTotal - this.stepLeft]);
            }
            this.currentLoopActions[this.stepTotal - this.stepLeft] = Direction.None;
            StepCost();
        }

        private void StepCost()
        {
            stepLeft--;
            AudioManager.Instance.PlaySfx(ClipConstants.SFX_LevelComplete);
            GameplayEvent.OnPlayerStepTaken?.Invoke();
            if (this.stepLeft == 0)
            {
                StartCoroutine(this.WaitEndLoop());
            }
        }

        private IEnumerator WaitEndLoop()
        {
            yield return new WaitForSeconds(0.5f);
            if (!GameManager.Instance.IsLevelComplete)
            {
                this.OverLoop();
            }
        }

        private void OverLoop()
        {
            this.lastLoopActions = this.currentLoopActions;
            MapManager.Instance.LoopMapInit();
            GameplayEvent.OnLoopEnded?.Invoke();
        }

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

        private Point point;

        private Point shadowPoint;

        private int stepTotal;

        private int stepLeft;

        private Direction[] currentLoopActions;

        private Direction[] lastLoopActions;
    }
}
