using System;

namespace Events
{
    public class GameplayEvent
    {
        public static Action OnPlayerStepTaken;
        public static Action OnLoopEnded;
        public static Action<string> OnPlayerLevelChanged;
    }
}