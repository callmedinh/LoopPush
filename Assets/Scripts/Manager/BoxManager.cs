using System.Collections.Generic;
using Controller;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class BoxManager : Singleton<BoxManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void AddBoxToList(BoxController bc)
        {
            this.boxList.Add(bc);
        }

        public BoxController GetBox(Point p)
        {
            for (int i = 0; i < this.boxList.Count; i++)
            {
                if (this.boxList[i].P == p)
                {
                    return this.boxList[i];
                }
            }
            return null;
        }

        public BoxController GetBox(int x, int y)
        {
            return this.GetBox(MapManager.Instance.GetPoint(x, y));
        }
        public void ClearBoxPoint()
        {
            for (int i = 0; i < this.boxList.Count; i++)
            {
                this.boxList[i].P.IsBox = false;
            }
        }
        public void ClearBox()
        {
            this.boxList.Clear();
        }
        public void RecoverBoxPosition(Vector2Int[] boxPositions)
        {
            if (boxPositions.Length != this.boxList.Count)
            {
                return;
            }
            for (int i = 0; i < boxPositions.Length; i++)
            {
                this.boxList[i].Init(boxPositions[i]);
            }
        }

        public void DetectBoxState()
        {
            for (int i = 0; i < this.boxList.Count; i++)
            {
                if (!this.boxList[i].IsSettled)
                {
                    return;
                }
            }
            GameManager.Instance.LevelComplete();
        }
        private List<BoxController> boxList = new List<BoxController>();
    }
}
