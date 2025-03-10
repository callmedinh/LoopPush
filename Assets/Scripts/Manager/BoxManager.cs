using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance
    {
        get
        {
            return BoxManager.instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        BoxManager.instance = this;
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
            this.boxList[i].P.isBox = false;
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
            Debug.LogError("????");
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

    /*
    public bool BoxReady()
    {
        bool result = true;
        Debug.Log(this.boxList.Count);
        for (int i = 0; i < this.boxList.Count; i++)
        {
            if (!this.boxList[i].IsSettled)
            {
                result = false;
            }
        }
        return result;
    }

    */
    private static BoxManager instance;
    private List<BoxController> boxList = new List<BoxController>();
}
