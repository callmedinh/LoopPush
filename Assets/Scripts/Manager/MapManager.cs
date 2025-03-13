using System;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance
    {
        get
        {
            return MapManager.instance;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
    }

    public void InitLevel(MapInfo map)
    {
        //this.keyblock = null;
        this.currentMap = map;
        this.mapWidth = map.mapSize.x;
        this.mapHeight = map.mapSize.y;
        this.mapPoints = new Point[this.mapWidth, this.mapHeight];

        for (int i = 0; i < map.blocks.Length; i++)
        {
            this.InstantiateBlock(map.blocks[i]);
        }
        for (int j = 0; j < map.teleportBlocks.Length; j++)
        {
            Point p;
            this.InstantiateBlock(map.teleportBlocks[j].block1, out p);
            Point p2;
            this.InstantiateBlock(map.teleportBlocks[j].block2, out p2);
            this.BindTeleportInfo(p, p2);
        }
        PlayerController.Instance.Init(map.playerPosition, map.steps);
        this.InstantiateBox(map.boxPositions);
        CameraController.Instance.SetPosition(map.mapSize);
        GamePanelManager.Instance.InitTimelineNode(map.steps);
        //GameObject prefab = this.smallTreeBrick;
        //MapType mapType = map.mapType;
        //if (mapType != MapType.SmallTree)
        //{
        //    if (mapType == MapType.BigTree)
        //    {
        //        prefab = this.bigTreeBrick;
        //    }
        //}
        //else
        //{
        //    prefab = this.smallTreeBrick;
        //}
        //this.InstantiateBrick(map.bricks, prefab);
        if (map.containStar)
        {
            this.starObject = Instantiate<GameObject>(this.starPrefab);
            this.starObject.transform.position = new Vector3((float)map.starPosition.x, (float)map.starPosition.y, 0f);
            this.GetPoint(map.starPosition.x, map.starPosition.y).isStar = true;
        }
        //this.bgObject = Object.Instantiate<GameObject>(map.bg, Camera.main.transform);
        //this.bgObject.transform.localPosition = Vector3.forward * 10f;
        //GamePanelManager.Instance.ShowLevelInfo(map.name, map.levelNameEn);
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
        MapInfo mapInfo = this.currentMap;
        PlayerController.Instance.Init(mapInfo.playerPosition, mapInfo.steps);
        BoxManager.Instance.ClearBoxPoint();
        BoxManager.Instance.RecoverBoxPosition(mapInfo.boxPositions);
        //this.StandKeyState(false);
        if (this.currentMap.containStar && this.starObject == null)
        {
            this.starObject = Instantiate<GameObject>(this.starPrefab);
            this.starObject.transform.position = new Vector3((float)mapInfo.starPosition.x, (float)mapInfo.starPosition.y, 0f);
            this.GetPoint(mapInfo.starPosition.x, mapInfo.starPosition.y).isStar = true;
        }
    }
    private void InstantiateBlock(MapBlock block)
    {
        int num = this.mapWidth * block.position.y + block.position.x;
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
        GameObject gameObject = Instantiate<GameObject>(original, new Vector3((float)block.position.x, (float)block.position.y, (float)num), Quaternion.identity, base.transform);
        //if (block.type == BlockType.Key)
        //{
        //    this.keyblock = gameObject.GetComponent<KeyBlockController>();
        //}
        this.blockObjList.Add(gameObject);
        gameObject.GetComponent<BlockController>().SetTypeBlock(block.type);
        Point point = new(block.position.x, block.position.y);
        this.mapPoints[block.position.x, block.position.y] = point;
        point.Position = gameObject.transform.position;
        if (block.type == BlockType.Teleport)
        {
            point.isTele = true;
            return;
        }
        if (block.type == BlockType.End)
        {
            point.isDst = true;
            return;
        }
        if (block.type == BlockType.Key)
        {
            point.isKey = true;
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
        int num = this.mapWidth * block.position.y + block.position.x;
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
        GameObject gameObject = Instantiate<GameObject>(original, new Vector3((float)block.position.x, (float)block.position.y, (float)num), Quaternion.identity, base.transform);
        //if (block.type == BlockType.Key)
        //{
        //    this.keyblock = gameObject.GetComponent<KeyBlockController>();
        //}
        this.blockObjList.Add(gameObject);
        gameObject.GetComponent<BlockController>().SetTypeBlock(block.type);
        p = new Point(block.position.x, block.position.y);
        this.mapPoints[block.position.x, block.position.y] = p;
        p.Position = gameObject.transform.position;
        if (block.type == BlockType.Teleport)
        {
            p.isTele = true;
            return;
        }
        if (block.type == BlockType.End)
        {
            p.isDst = true;
            return;
        }
        if (block.type == BlockType.Key)
        {
            p.isKey = true;
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

    /*
    private void InstantiateBrick(MapBlock[] blocks, GameObject prefab)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            int num = this.mapWidth * blocks[i].position.y + blocks[i].position.x;
            GameObject item = Object.Instantiate<GameObject>(prefab, new Vector3((float)blocks[i].position.x, (float)blocks[i].position.y, (float)num), Quaternion.identity, base.transform);
            this.brickObjList.Add(item);
        }
    }
    */

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
        Debug.Log("Get Point: " + this.mapPoints[x, y]);
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

    /*
    public void StandKeyState(bool state)
    {
        if (this.keyblock == null)
        {
            return;
        }
        this.keyblock.SetStandState(state);
    }
    */
    private static MapManager instance;

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
    private GameObject smallTreeBrick;

    [SerializeField]
    private GameObject bigTreeBrick;

    [SerializeField]
    private GameObject starPrefab;

    [SerializeField]
    private Transform playerTransform;

    private Point[,] mapPoints;

    private int mapWidth;

    private int mapHeight;

    private MapInfo currentMap;

    private List<GameObject> blockObjList = new List<GameObject>();

    private List<GameObject> boxObjList = new List<GameObject>();

    private List<GameObject> brickObjList = new List<GameObject>();

    private GameObject bgObject;

    private GameObject starObject;
}
