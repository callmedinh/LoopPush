using System;

namespace Events
{
    public static class InputEvent
    {
        public static Action OnLeftDirectionPressed;
        public static Action OnRightDirectionPressed;
        public static Action OnUpDirectionPressed;
        public static Action OnDownDirectionPressed;

        public static Action OnSkipTurnButtonPressed;
    }
}