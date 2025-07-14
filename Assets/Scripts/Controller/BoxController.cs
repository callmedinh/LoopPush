using DG.Tweening;
using Manager;
using UnityEngine;

namespace Controller
{
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
            if (this.point == null)
            {
                Debug.LogError($"[BoxController] Box position invalid: ({position.x}, {position.y}) — no block at that position.");
                return;
            }
            transform.position = new Vector3((float)position.x, (float)position.y, -2f);
            this.point.IsBox = true;
            this.isSettled = false;
        }


        public void Move(Direction dir)
        {
            Point _point = null;
            switch (dir)
            {
                case Direction.Left:
                    _point = MapManager.Instance.GetPoint(this.point.X - 1, this.point.Y);
                    break;
                case Direction.Right:
                    _point = MapManager.Instance.GetPoint(this.point.X + 1, this.point.Y);
                    break;
                case Direction.Up:
                    _point = MapManager.Instance.GetPoint(this.point.X, this.point.Y + 1);
                    break;
                case Direction.Down:
                    _point = MapManager.Instance.GetPoint(this.point.X, this.point.Y - 1);
                    break;
            }

            if (_point == null) return;

            // Teleport logic
            if (_point.IsTele && !_point.TelePoint.IsBox)
            {
                _point = _point.TelePoint;
            }

            this.point.IsBox = false;
            transform.DOMove(_point.Position, 0.1f);
            this.point = _point;
            this.point.IsBox = true;

            // Destination check
            this.isSettled = this.point.IsDst;
            if (this.isSettled)
            {
                BoxManager.Instance.DetectBoxState();
            }
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
}
