using DG.Tweening;
using Manager;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class MovableBase : MonoBehaviour
    {
        protected Point currentPoint;
        protected virtual float moveDuration => 0.1f;

        public virtual void Init(Vector2Int position, float zOffset = -1f)
        {
            currentPoint = MapManager.Instance.GetPoint(position.x, position.y);
            transform.position = currentPoint.Position + new Vector3(0f, 0f, zOffset);
        }

        public virtual Point GetTargetPoint(Direction dir)
        {
            var offset = dir.ToOffset();
            return MapManager.Instance.GetPoint(currentPoint.X + offset.x, currentPoint.Y + offset.y);
        }

        public virtual Point ResolveTeleport(Point point)
        {
            if (point.IsTele && !point.TelePoint.IsBox)
                return point.TelePoint;
            return point;
        }

        protected virtual void AnimateMove(Point target, float zOffset = -1f)
        {
            transform.DOMove(target.Position + new Vector3(0f, 0f, zOffset), moveDuration);
            currentPoint = target;
        }
    }

    public static class DirectionExtensions
    {
        public static Vector2Int ToOffset(this Direction dir)
        {
            return dir switch
            {
                Direction.Left => Vector2Int.left,
                Direction.Right => Vector2Int.right,
                Direction.Up => Vector2Int.up,
                Direction.Down => Vector2Int.down,
                _ => Vector2Int.zero
            };
        }
    }
}