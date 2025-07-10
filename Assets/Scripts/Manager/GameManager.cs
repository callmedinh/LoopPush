using System.Collections;
using Controller;
using UI;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public bool IsLevelComplete
        {
            get
            {
                return this.isLevelComplete;
            }
        }

        public void EnterLevel(string levelID)
        {
            PlayerController.Instance.ClearLastLoop();
            this.isLevelComplete = false;
            InputManager.Enabled = true;
            this.enteredLevel = Resources.Load<MapInfo>("Levels/" + levelID);
            MapManager.Instance.InitLevel(enteredLevel);
            for (int i = 0; i < this.enteredLevel.blocks.Length; i++)
            {
                if (this.enteredLevel.blocks[i].type == BlockType.Key)
                {
                    this.keyPoint = MapManager.Instance.GetKeyPoint(enteredLevel);
                    break;
                }
            }
            string[] array = this.enteredLevel.name.Split(new char[]
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
            MapManager.Instance.InitLevel(this.enteredLevel);
            this.keyPoint = MapManager.Instance.GetKeyPoint(this.enteredLevel);
        }


        public void LevelComplete()
        {
            this.isLevelComplete = true;
            InputManager.Enabled = false;
            StartCoroutine(this.WaitBackToLevel());
            DeData.Instance.SaveLevel();
        }
        private IEnumerator WaitBackToLevel()
        {
            yield return new WaitForSeconds(0.9f);
            UIManager.Instance.ShowView(ViewConstants.LevelView);
            MapManager.Instance.ClearLevel();
            yield break;
        }
        public void BackToMenu()
        {
            InputManager.Enabled = false;
            UIManager.Instance.ShowView(ViewConstants.LevelView);
            MapManager.Instance.ClearLevel();
        }
        private bool isLevelComplete;
        private MapInfo enteredLevel;
        private Point keyPoint;
    }
}
