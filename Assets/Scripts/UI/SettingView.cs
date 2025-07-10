using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingView : UIBaseView
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Slider sfxSlider;
        [SerializeField] private Slider musicSlider;

        private void Awake()
        {
            sfxSlider.onValueChanged.AddListener(val => SettingEvents.OnSfxVolumeChanged?.Invoke(val));
            musicSlider.onValueChanged.AddListener(val => SettingEvents.OnMusicVolumeChanged?.Invoke(val));

            closeButton.onClick.AddListener(() =>
            {
                SettingEvents.OnSettingsClosed?.Invoke();
                Hide();
            });
        }
    }
}