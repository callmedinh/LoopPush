using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance
    {
        get
        {
            return _instance;
        }
    }
    private static CameraController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public void SetPosition(Vector2Int mapSize)
    {
        transform.position = new Vector3((float)(mapSize.x - 1) / 2f, (float)(mapSize.y - 1) / 2, -10f);
    }
}
