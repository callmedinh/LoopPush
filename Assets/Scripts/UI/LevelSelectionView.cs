using Audio;
using DG.Tweening;
using Events;
using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using Button = UnityEngine.UI.Button;

namespace UI
{
    public class LevelSelectionView : UIBaseView
    {
        [SerializeField] private Sprite openedSprite;
        [SerializeField] private Sprite lockedSprite;
        [SerializeField] private Button buttonPrefab;
        [SerializeField] private Transform buttonParent;
        [SerializeField] private CanvasGroup canvasGroup;
        private MapInfo[] maps;

        private void OnEnable()
        {
            AudioManager.Instance.PlayBGMMusic(ClipConstants.BGM_LevelMenu);
            ClearAllButtons();
            if (DeData.Instance == null) return;
            DeData.Instance.Initialize();
            maps = Resources.LoadAll<MapInfo>("Levels/");
            for (var i = 0; i < maps.Length; i++)
            {
                var chapter = i / 4 + 1;
                var level = i % 4;
                var levelId = $"{chapter}-{level}";
                var button = Instantiate(buttonPrefab, buttonParent);
                var tmp = button.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = levelId;
                button.interactable = DeData.Instance.GetLevelData(chapter, level).CanStart;

                var image = button.GetComponentInChildren<Image>();
                if (image != null)
                    image.sprite = DeData.Instance.GetLevelData(chapter, level).CanStart ? openedSprite : lockedSprite;
                button.onClick.AddListener(() => OnLevelClick(levelId));
            }
        }

        private void OnDisable()
        {
            AudioManager.Instance.StopBGMMusic();
        }

        private void OnLevelClick(string level)
        {
            GameManager.Instance.EnterLevel(level);
            GameplayEvent.OnPlayerLevelChanged?.Invoke(level);
            UIManager.Instance.ShowView(ScreenType.Gameplay);
        }

        private void ClearAllButtons()
        {
            foreach (var btn in buttonParent.GetComponentsInChildren<Button>()) Destroy(btn.gameObject);
        }

        public override void OnShow()
        {
            base.OnShow();
            canvasGroup.alpha = 0f;
            var rect = canvasGroup.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -Screen.height);

            rect.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.OutCubic);
            canvasGroup.DOFade(1f, 0.5f);
        }
    }
}