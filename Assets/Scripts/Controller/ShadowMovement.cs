using DefaultNamespace;
using Manager;

namespace Controller
{
    public class ShadowMovement : MovableBase
    {
        public void MoveTo(Direction dir)
        {
            var target = GetTargetPoint(dir);
            if (target == null) return;
            if (target.IsBox)
            {
                if (!BoxManager.Instance.GetBox(target).CanMove(dir))
                    return;
                BoxManager.Instance.GetBox(target).Move(dir);
            }

            if (target.IsStar) MapManager.Instance.DestroyStar();
            target = ResolveTeleport(target);
            AnimateMove(target);
        }
    }
}