using UnityEngine;

public class GamePanelManager : MonoBehaviour
{
    private static GamePanelManager _instance;
    public static GamePanelManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != this && _instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
