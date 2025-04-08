using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelManager : MonoBehaviour
{
    [SerializeField]
    private Text levelIDText;
    [SerializeField]
    private Text levelNameText;
    private int nodeCount;
    private int currentNode;
    private List<NodeController> nodeObjList = new List<NodeController>();
    [SerializeField]
    private GameObject nodePrefab;
    [SerializeField]
    private Transform nodeGridTransform;
    private static GamePanelManager _instance;
    [SerializeField]
    private Transform timelineTransform;
    public static GamePanelManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != this && _instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        Hide();
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void InitTimelineNode(int count)
    {
        this.nodeCount = count;
        this.currentNode = 0;
        this.ClearTimelineNode();
        for (int i = 0; i < count; i++)
        {
            NodeController component = Object.Instantiate<GameObject>(this.nodePrefab, this.nodeGridTransform).GetComponent<NodeController>();
            this.nodeObjList.Add(component);
        }
        float x = (float)(-36 * count);
        this.timelineTransform.localPosition = new Vector3(x, 180f, 0f);
    }
    public void ClearTimelineNode()
    {
        for (int i = 0; i < this.nodeObjList.Count; i++)
        {
            Destroy(this.nodeObjList[i].gameObject);
        }
        this.nodeObjList.Clear();
    }
    public void StepOneNode()
    {
        this.nodeObjList[this.currentNode].DoFill();
        this.currentNode++;
    }
    public void ClearNodeStep()
    {
        for (int i = 0; i < this.nodeObjList.Count; i++)
        {
            this.nodeObjList[i].ClearFill();
        }
        this.currentNode = 0;
    }
}
