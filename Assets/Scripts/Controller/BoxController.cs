using DefaultNamespace;
using Manager;
using UnityEngine;

namespace Controller
{
    public class BoxController : MovableBase
    {
        private bool isSettled;
        public bool IsSettled => currentPoint.IsDst;
        public Point Point => currentPoint;

        public override void Init(Vector2Int position, float zOffset = -2f)
        {
            base.Init(position);
            currentPoint.IsBox = true;
            isSettled = false;
        }


        public void Move(Direction dir)
        {
            var target = GetTargetPoint(dir);
            if (target == null) return;

            target = ResolveTeleport(target);

            currentPoint.IsBox = false;
            AnimateMove(target, -2f);
            currentPoint = target;
            currentPoint.IsBox = true;

            isSettled = currentPoint.IsDst;
            if (isSettled)
                BoxManager.Instance.DetectBoxState();
        }

        public bool CanMove(Direction dir)
        {
            var target = GetTargetPoint(dir);
            return target != null;
        }
    }
}