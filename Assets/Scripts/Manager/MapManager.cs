using System.Collections.Generic;
using Controller;
using UnityEngine;
using Utilities;

namespace Manager
{
    public class MapManager : Singleton<MapManager>
    {
        public static MapInfo CurrentMap;
        [SerializeField] private Pool pool;

        [SerializeField] private MapInfo testMap;

        [SerializeField] private GameObject bigTreeBrick;

        [SerializeField] private GameObject starPrefab;

        [SerializeField] private Transform playerTransform;

        private readonly List<GameObject> blockObjList = new();

        private GameObject bgObject;

        private int mapHeight;

        private Point[,] mapPoints;

        private int mapWidth;

        private GameObject starObject;

        public void InitLevel(MapInfo map)
        {
            CurrentMap = map;
            mapWidth = map.mapSize.x;
            mapHeight = map.mapSize.y;
            mapPoints = new Point[mapWidth, mapHeight];

            for (var i = 0; i < map.blocks.Length; i++) InstantiateBlock(map.blocks[i]);
            for (var j = 0; j < map.teleportBlocks.Length; j++)
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
                starObject = Instantiate(starPrefab);
                starObject.transform.position = new Vector3(map.starPosition.x, map.starPosition.y, 0f);
                GetPoint(map.starPosition.x, map.starPosition.y).IsStar = true;
            }

            if (map.bg != null)
            {
                if (Camera.main != null) bgObject = Instantiate(map.bg, Camera.main.transform);
                bgObject.transform.localPosition = Vector3.forward * 10f;
            }
        }

        public void ClearLevel()
        {
            pool.ReturnListToPool(blockObjList);
            blockObjList.Clear();
            Destroy(bgObject);
            Destroy(starObject);
            BoxManager.Instance.ClearBox();
        }

        public void LoopMapInit()
        {
            var mapInfo = CurrentMap;
            PlayerController.Instance.LoopInit(mapInfo.playerPosition, mapInfo.steps);
            BoxManager.Instance.ClearBoxPoint();
            BoxManager.Instance.RecoverBoxPosition(mapInfo.boxPositions);
            if (CurrentMap.containStar && starObject == null)
            {
                starObject = Instantiate(starPrefab);
                starObject.transform.position = new Vector3(mapInfo.starPosition.x, mapInfo.starPosition.y, 0f);
                GetPoint(mapInfo.starPosition.x, mapInfo.starPosition.y).IsStar = true;
            }
        }

        private void InstantiateBlock(MapBlock block)
        {
            var obj = pool.GetPooledObject(block.type);
            obj.transform.position = new Vector3(block.position.x, block.position.y, -1f);
            blockObjList.Add(obj);
            Point point = new(block.position.x, block.position.y);
            if (block.position.x < 0 || block.position.x >= mapWidth || block.position.y < 0 ||
                block.position.y >= mapHeight) return;
            mapPoints[block.position.x, block.position.y] = point;
            point.Position = obj.transform.position;
            ApplyBlockType(block.type, point);
        }

        public void DestroyStar()
        {
            if (starObject == null) return;
            Destroy(starObject);
        }

        private void InstantiateBlock(MapBlock block, out Point p)
        {
            var obj = pool.GetPooledObject(block.type);
            obj.transform.position = new Vector3(block.position.x, block.position.y, -1f);
            blockObjList.Add(obj);
            p = new Point(block.position.x, block.position.y);
            mapPoints[block.position.x, block.position.y] = p;
            p.Position = obj.transform.position;
            ApplyBlockType(block.type, p);
        }

        private void ApplyBlockType(BlockType type, Point p)
        {
            switch (type)
            {
                case BlockType.Teleport:
                    p.IsTele = true;
                    break;
                case BlockType.End:
                    p.IsDst = true;
                    break;
                case BlockType.Key:
                    p.IsKey = true;
                    break;
            }
        }


        private void InstantiateBox(Vector2Int[] boxPositions)
        {
            for (var i = 0; i < boxPositions.Length; i++)
            {
                var obj = pool.GetPooledObject(BlockType.Box);
                blockObjList.Add(obj);
                var component = obj.GetComponent<BoxController>();
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
            if (x < 0 || x > mapWidth - 1 || y < 0 || y > mapHeight - 1) return null;
            return mapPoints[x, y];
        }

        public Point GetKeyPoint(MapInfo level)
        {
            for (var i = 0; i < level.blocks.Length; i++)
                if (level.blocks[i].type == BlockType.Key)
                    return GetPoint(level.blocks[i].position.x, level.blocks[i].position.y);

            return null;
        }
    }
}