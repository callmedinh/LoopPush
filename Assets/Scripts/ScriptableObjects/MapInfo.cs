using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "MapInfo")]
public class MapInfo : ScriptableObject
{
    public Vector2Int mapSize;

    public MapBlock[] blocks;

    public TeleportBlock[] teleportBlocks;

    public MapBlock[] bricks;

    public Vector2Int playerPosition;

    public Vector2Int[] boxPositions;

    public bool containStar;

    public Vector2Int starPosition;

    public int steps;

    public GameObject bg;

    public string levelName;
}