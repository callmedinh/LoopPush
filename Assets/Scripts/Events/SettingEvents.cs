using System;

namespace Events
{
    public class SettingEvents
    {
        public static Action<float> OnSfxVolumeChanged;
        public static Action<float> OnMusicVolumeChanged;
        public static Action OnSettingsClosed;
    }
}