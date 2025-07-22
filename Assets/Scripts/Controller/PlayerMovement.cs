using DefaultNamespace;
using Manager;
using UnityEngine;

namespace Controller
{
    public class PlayerMovement : MovableBase
    {
        [SerializeField] private Animator animator;

        public void Init(Vector2Int position)
        {
            currentPoint = MapManager.Instance.GetPoint(position.x, position.y);
            transform.position = currentPoint.Position + new Vector3(0f, 0f, -1f);
        }

        public bool TryMove(Direction dir, out bool teleported)
        {
            teleported = false;
            var target = GetTargetPoint(dir);
            if (target == null) return false;

            if (target.IsBox && !BoxManager.Instance.GetBox(target).CanMove(dir)) return false;

            if (target.IsBox)
            {
                BoxManager.Instance.GetBox(target).Move(dir);
                if (dir == Direction.Left || dir == Direction.Right)
                    animator.SetTrigger("Slide");
            }

            target = ResolveTeleport(target);
            AnimateMove(target);
            return true;
        }
    }
}