using UnityEngine;
using Utilities;

public class CameraController : Singleton<CameraController>
{
    public void SetPosition(Vector2Int mapSize)
    {
        transform.position = new Vector3((float)(mapSize.x - 1) / 2f, (float)(mapSize.y - 1) / 2, -10f);
    }
}
