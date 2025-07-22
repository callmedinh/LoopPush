using System.Collections.Generic;
using Controller;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class BoxManager : Singleton<BoxManager>
    {
        private readonly List<BoxController> boxList = new();

        protected override void Awake()
        {
            base.Awake();
        }

        public void AddBoxToList(BoxController bc)
        {
            boxList.Add(bc);
        }

        public BoxController GetBox(Point p)
        {
            for (var i = 0; i < boxList.Count; i++)
                if (boxList[i].Point == p)
                    return boxList[i];

            return null;
        }

        public BoxController GetBox(int x, int y)
        {
            return GetBox(MapManager.Instance.GetPoint(x, y));
        }

        public void ClearBoxPoint()
        {
            for (var i = 0; i < boxList.Count; i++) boxList[i].Point.IsBox = false;
        }

        public void ClearBox()
        {
            boxList.Clear();
        }

        public void RecoverBoxPosition(Vector2Int[] boxPositions)
        {
            if (boxPositions.Length != boxList.Count) return;
            for (var i = 0; i < boxPositions.Length; i++) boxList[i].Init(boxPositions[i]);
        }

        public void DetectBoxState()
        {
            for (var i = 0; i < boxList.Count; i++)
                if (!boxList[i].IsSettled)
                    return;

            GameManager.Instance.LevelComplete();
        }
    }
}