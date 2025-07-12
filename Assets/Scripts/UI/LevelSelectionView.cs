using System;
using Audio;
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
        private MapInfo[] maps;
        private void OnDisable()
        {
            AudioManager.Instance.StopBGMMusic();
        }

        private void OnEnable() 
        {   
            AudioManager.Instance.PlayBGMMusic(ClipConstants.BGM_LevelMenu);
            ClearAllButtons();
            if (DeData.Instance == null) return;
            DeData.Instance.Initialize();
            maps = Resources.LoadAll<MapInfo>("Levels/");
            for (int i = 0; i < maps.Length; i++)
            {
                int chapter = i / 4 + 1;
                int level = i % 4;
                string levelId = $"{chapter}-{level}";
                Button button = Instantiate(buttonPrefab, buttonParent);
                TextMeshProUGUI tmp = button.GetComponentInChildren<TextMeshProUGUI>();
                if (tmp != null) tmp.text = levelId;
                button.interactable = DeData.Instance.GetLevelData(chapter, level).CanStart;
                
                Image image = button.GetComponentInChildren<Image>();
                if (image != null)
                {
                    image.sprite = (DeData.Instance.GetLevelData(chapter, level).CanStart ? this.openedSprite : this.lockedSprite);
                }
                button.onClick.AddListener(() => OnLevelClick(levelId));
            }
        }

        private void OnLevelClick(string level)
        {
            GameManager.Instance.EnterLevel(level);
            GameplayEvent.OnPlayerLevelChanged?.Invoke(level);
            UIManager.Instance.ShowView(ViewConstants.GameplayView);
        }

        private void ClearAllButtons()
        {
            foreach (var btn in buttonParent.GetComponentsInChildren<Button>())
            {
                Destroy(btn.gameObject);
            }
        }
    }
}