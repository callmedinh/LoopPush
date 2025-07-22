using Audio;
using Events;
using Utilities;

namespace DefaultNamespace
{
    public class StepSystem
    {
        public StepSystem(int total)
        {
            Total = total;
            Left = total;
        }

        public int Total { get; }
        public int Left { get; private set; }
        public int CurrentStepIndex => Total - Left;
        public bool IsLastStep => Left <= 0;

        public bool CanMove()
        {
            return Left > 0;
        }

        public void UseStep()
        {
            Left--;
            AudioManager.Instance.PlaySfx(ClipConstants.SFX_LevelComplete);
            GameplayEvent.OnPlayerStepTaken?.Invoke();
        }

        public void Reset()
        {
            Left = Total;
        }
    }
}