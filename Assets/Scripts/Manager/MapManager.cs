using System.Collections.Generic;
using Controller;
using UI;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class MapManager : Singleton<MapManager>
    {
        [SerializeField] private GameplayView gameplayView;
        protected override void Awake()
        {
            base.Awake();
        }

        public void InitLevel(MapInfo map)
        {
            CurrentMap = map;
            mapWidth = map.mapSize.x;
            mapHeight = map.mapSize.y;
            mapPoints = new Point[this.mapWidth, this.mapHeight];

            for (int i = 0; i < map.blocks.Length; i++)
            {
                InstantiateBlock(map.blocks[i]);
            }
            for (int j = 0; j < map.teleportBlocks.Length; j++)
            {
                Point p;
                InstantiateBlock(map.teleportBlocks[j].block1, out p);
                Point p2;
                InstantiateBlock(map.teleportBlocks[j].block2, out p2);
                BindTeleportInfo(p, p2);
            }
            PlayerController.Instance.Init(map.playerPosition, map.steps);
            InstantiateBox(map.boxPositions);
            CameraController.Instance.SetPosition(map.mapSize);
            if (map.containStar)
            {
                this.starObject = Instantiate(this.starPrefab);
                this.starObject.transform.position = new Vector3((float)map.starPosition.x, (float)map.starPosition.y, 0f);
                this.GetPoint(map.starPosition.x, map.starPosition.y).IsStar = true;
            }

            if (map.bg != null)
            {
                bgObject = Instantiate<GameObject>(map.bg, Camera.main.transform);
                bgObject.transform.localPosition = Vector3.forward * 10f;   
            }
        }

        public void ClearLevel()
        {
            for (int i = 0; i < this.blockObjList.Count; i++)
            {
                Destroy(this.blockObjList[i]);
            }
            for (int j = 0; j < this.boxObjList.Count; j++)
            {
                Destroy(this.boxObjList[j]);
            }
            for (int k = 0; k < this.brickObjList.Count; k++)
            {
                Destroy(this.brickObjList[k]);
            }
            this.blockObjList.Clear();
            this.boxObjList.Clear();
            this.brickObjList.Clear();
            Destroy(this.bgObject);
            Destroy(this.starObject);
            BoxManager.Instance.ClearBox();
        }

        public void LoopMapInit()
        {
            MapInfo mapInfo = CurrentMap;
            PlayerController.Instance.Init(mapInfo.playerPosition, mapInfo.steps);
            BoxManager.Instance.ClearBoxPoint();
            BoxManager.Instance.RecoverBoxPosition(mapInfo.boxPositions);
            if (CurrentMap.containStar && this.starObject == null)
            {
                this.starObject = Instantiate<GameObject>(this.starPrefab);
                this.starObject.transform.position = new Vector3((float)mapInfo.starPosition.x, (float)mapInfo.starPosition.y, 0f);
                this.GetPoint(mapInfo.starPosition.x, mapInfo.starPosition.y).IsStar = true;
            }
        }
        private void InstantiateBlock(MapBlock block)
        {
            GameObject original = this.blockPrefab;
            switch (block.type)
            {
                case BlockType.End:
                    original = this.endPrefab;
                    break;
                case BlockType.Teleport:
                    original = this.telePrefab;
                    break;
                case BlockType.Key:
                    original = this.keyPrefab;
                    break;
            }
            GameObject obj = Instantiate<GameObject>(original, new Vector3((float)block.position.x, (float)block.position.y, -1f), Quaternion.identity, base.transform);
            this.blockObjList.Add(obj);
            obj.GetComponent<BlockController>().SetTypeBlock(block.type);
            Point point = new(block.position.x, block.position.y);
            this.mapPoints[block.position.x, block.position.y] = point;
            point.Position = obj.transform.position;
            if (block.type == BlockType.Teleport)
            {
                point.IsTele = true;
                return;
            }
            if (block.type == BlockType.End)
            {
                point.IsDst = true;
                return;
            }
            if (block.type == BlockType.Key)
            {
                point.IsKey = true;
            }
        }

        public void DestroyStar()
        {
            if (this.starObject == null)
            {
                return;
            }
            Destroy(this.starObject);
        }

        private void InstantiateBlock(MapBlock block, out Point p)
        {
            GameObject original = this.blockPrefab;
            switch (block.type)
            {
                case BlockType.End:
                    original = this.endPrefab;
                    break;
                case BlockType.Teleport:
                    original = this.telePrefab;
                    break;
                case BlockType.Key:
                    original = this.keyPrefab;
                    break;
            }
            GameObject gameObject = Instantiate<GameObject>(original, new Vector3((float)block.position.x, (float)block.position.y, -1f), Quaternion.identity, base.transform);
            this.blockObjList.Add(gameObject);
            gameObject.GetComponent<BlockController>().SetTypeBlock(block.type);
            p = new Point(block.position.x, block.position.y);
            this.mapPoints[block.position.x, block.position.y] = p;
            p.Position = gameObject.transform.position;
            if (block.type == BlockType.Teleport)
            {
                p.IsTele = true;
                return;
            }
            if (block.type == BlockType.End)
            {
                p.IsDst = true;
                return;
            }
            if (block.type == BlockType.Key)
            {
                p.IsKey = true;
            }
        }

        private void InstantiateBox(Vector2Int[] boxPositions)
        {
            for (int i = 0; i < boxPositions.Length; i++)
            {
                GameObject gameObject = Instantiate<GameObject>(this.boxPrefab, base.transform);
                this.boxObjList.Add(gameObject);
                BoxController component = gameObject.GetComponent<BoxController>();
                component.Init(boxPositions[i]);
                BoxManager.Instance.AddBoxToList(component);
            }
        }

        private void BindTeleportInfo(Point p1, Point p2)
        {
            p1.TelePoint = p2;
            p2.TelePoint = p1;
        }

        public Point GetPoint(int x, int y)
        {
            if (x < 0 || x > this.mapWidth - 1 || y < 0 || y > this.mapHeight - 1)
            {
                return null;
            }
            return this.mapPoints[x, y];
        }

        public Point GetKeyPoint(MapInfo level)
        {
            for (int i = 0; i < level.blocks.Length; i++)
            {
                if (level.blocks[i].type == BlockType.Key)
                {
                    return this.GetPoint(level.blocks[i].position.x, level.blocks[i].position.y);
                }
            }
            return null;
        }
        [SerializeField]
        private MapInfo testMap;

        [SerializeField]
        private GameObject blockPrefab;

        [SerializeField]
        private GameObject endPrefab;

        [SerializeField]
        private GameObject telePrefab;

        [SerializeField]
        private GameObject keyPrefab;

        [SerializeField]
        private GameObject boxPrefab;

        [SerializeField]
        private GameObject bigTreeBrick;

        [SerializeField]
        private GameObject starPrefab;

        [SerializeField]
        private Transform playerTransform;

        private Point[,] mapPoints;

        private int mapWidth;

        private int mapHeight;

        public static MapInfo CurrentMap;

        private List<GameObject> blockObjList = new List<GameObject>();

        private List<GameObject> boxObjList = new List<GameObject>();

        private List<GameObject> brickObjList = new List<GameObject>();

        private GameObject bgObject;

        private GameObject starObject;
    }
}
