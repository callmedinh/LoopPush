using System.Collections;
using Audio;
using Controller;
using Events;
using UI;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        private MapInfo enteredLevel;
        private Point keyPoint;
        public bool IsLevelComplete { get; private set; }

        public void EnterLevel(string levelID)
        {
            IsLevelComplete = false;
            InputManager.Enabled = true;
            enteredLevel = Resources.Load<MapInfo>("Levels/" + levelID);
            MapManager.Instance.InitLevel(enteredLevel);
            for (var i = 0; i < enteredLevel.blocks.Length; i++)
                if (enteredLevel.blocks[i].type == BlockType.Key)
                {
                    keyPoint = MapManager.Instance.GetKeyPoint(enteredLevel);
                    break;
                }

            var array = enteredLevel.name.Split(new[]
            {
                '-'
            });
            DeData.Instance.SetEnterLevel(int.Parse(array[0]), int.Parse(array[1]));
        }

        public void RestartLevel()
        {
            InputManager.Enabled = false;
            MapManager.Instance.ClearLevel();
            PlayerController.Instance.ClearLastLoop();
            InputManager.Enabled = true;
            MapManager.Instance.InitLevel(enteredLevel);
            GameplayEvent.OnLoopEnded?.Invoke();
            keyPoint = MapManager.Instance.GetKeyPoint(enteredLevel);
        }


        public void LevelComplete()
        {
            AudioManager.Instance.PlaySfx(ClipConstants.SFX_LevelComplete);
            IsLevelComplete = true;
            InputManager.Enabled = false;
            StartCoroutine(WaitBackToLevel());
            DeData.Instance.SaveLevel();
        }

        private IEnumerator WaitBackToLevel()
        {
            yield return new WaitForSeconds(0.9f);
            BackToMenu();
            yield break;
        }

        public void BackToMenu()
        {
            InputManager.Enabled = false;
            UIManager.Instance.ShowView(ScreenType.LevelSelect);
            MapManager.Instance.ClearLevel();
        }
    }
}