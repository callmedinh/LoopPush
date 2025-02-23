using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000003 RID: 3
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
        BoxManager.instance = this;
    }

    // Token: 0x0600000A RID: 10 RVA: 0x0000232F File Offset: 0x0000052F
    public void AddBoxToList(BoxController bc)
    {
        this.boxList.Add(bc);
    }

    // Token: 0x0600000B RID: 11 RVA: 0x00002340 File Offset: 0x00000540
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

    // Token: 0x0600000C RID: 12 RVA: 0x00002385 File Offset: 0x00000585
    public BoxController GetBox(int x, int y)
    {
        return this.GetBox(MapManager.Instance.GetPoint(x, y));
    }

    /*
    public void ClearBoxPoint()
    {
        for (int i = 0; i < this.boxList.Count; i++)
        {
            this.boxList[i].P.isBox = false;
        }
    }
    */

    // Token: 0x0600000E RID: 14 RVA: 0x000023D6 File Offset: 0x000005D6
    public void ClearBox()
    {
        this.boxList.Clear();
    }

    /*
    // Token: 0x0600000F RID: 15 RVA: 0x000023E4 File Offset: 0x000005E4
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

    // Token: 0x06000010 RID: 16 RVA: 0x00002434 File Offset: 0x00000634
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

    // Token: 0x06000011 RID: 17 RVA: 0x00002478 File Offset: 0x00000678
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
    // Token: 0x04000005 RID: 5
    private static BoxManager instance;

    // Token: 0x04000006 RID: 6
    private List<BoxController> boxList = new List<BoxController>();
}
