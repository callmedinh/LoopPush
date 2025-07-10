using System;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using DG.Tweening;

namespace UI
{
    public class TitleView : UIBaseView
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button settingButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
            if (startButton != null)
            {
                startButton.transform.DOScale(1.3f, 0.5f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
            }
        }

        private void OnStartButtonClicked()
        {
            UIManager.Instance.ShowView(ViewConstants.LevelView);
        }
    }
}