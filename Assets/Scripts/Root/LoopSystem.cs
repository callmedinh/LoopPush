using System.Collections;
using Events;
using Manager;
using UnityEngine;

namespace DefaultNamespace
{
    public class LoopSystem
    {
        public LoopSystem(Direction[] directions)
        {
            CurrentLoop = directions;
            LastLoop = null;
        }

        public Direction[] CurrentLoop { get; private set; }
        public Direction[] LastLoop { get; private set; }

        public bool HasLastLoop()
        {
            return LastLoop != null;
        }

        public void SkipStep(int index)
        {
            CurrentLoop[index] = Direction.None;
        }

        private void OverLoop()
        {
            LastLoop = (Direction[])CurrentLoop.Clone();
            MapManager.Instance.LoopMapInit();
            GameplayEvent.OnLoopEnded?.Invoke();
        }

        public IEnumerator WaitEndLoop()
        {
            yield return new WaitForSeconds(0.5f);
            OverLoop();
        }

        public void ResetLoop()
        {
            CurrentLoop = LastLoop;
            LastLoop = null;
        }
    }
}