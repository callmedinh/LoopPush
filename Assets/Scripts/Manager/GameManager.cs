using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return GameManager.instance;
        }
    }

    private void Awake()
    {
        GameManager.instance = this;
        //DeData.Instance.Initialize();
    }

    public bool IsLevelComplete
    {
        get
        {
            return this.isLevelComplete;
        }
    }

    public void EnterLevel(string levelID)
    {
        PlayerController.Instance.ClearLastLoop();
        this.isLevelComplete = false;
        InputManager.Enabled = true;
        this.enteredLevel = Resources.Load<MapInfo>("Levels/" + levelID);
        MapManager.Instance.InitLevel(this.enteredLevel);
        this.levelContainKey = false;
        for (int i = 0; i < this.enteredLevel.blocks.Length; i++)
        {
            if (this.enteredLevel.blocks[i].type == BlockType.Key)
            {
                this.levelContainKey = true;
                this.keyPoint = MapManager.Instance.GetKeyPoint(this.enteredLevel);
                break;
            }
        }
        string[] array = this.enteredLevel.name.Split(new char[]
        {
            '-'
        });
    }

    /*

    public void RestartLevel()
    {
        InputManager.Enabled = false;
        FadeTransitionManager.Instance.Fade(delegate
        {
            MapManager.Instance.ClearLevel();
            PlayerController.Instance.ClearLastLoop();
            InputManager.Enabled = true;
            MapManager.Instance.InitLevel(this.enteredLevel);
            this.keyPoint = MapManager.Instance.GetKeyPoint(this.enteredLevel);
        });
    }

    // Token: 0x0600002D RID: 45 RVA: 0x00002A74 File Offset: 0x00000C74
    public void DetectStandKey()
    {
        bool state = this.keyPoint == PlayerController.Instance.SelfPoint || this.keyPoint == PlayerController.Instance.ShadowPoint;
        MapManager.Instance.StandKeyState(state);
    }

    */
    public void LevelComplete()
    {
        //if (this.levelContainKey)
        //{
        //    Debug.Log(this.keyPoint.ToString() + PlayerController.Instance.SelfPoint.ToString() + PlayerController.Instance.ShadowPoint.ToString());
        //    Debug.Log(BoxManager.Instance.BoxReady());
        //    if (!BoxManager.Instance.BoxReady() || (this.keyPoint != PlayerController.Instance.SelfPoint && this.keyPoint != PlayerController.Instance.ShadowPoint))
        //    {
        //        return;
        //    }
        //}
        this.isLevelComplete = true;
        Debug.Log("????");
        InputManager.Enabled = false;
        base.StartCoroutine(this.WaitBackToLevel());
    }
    private IEnumerator WaitBackToLevel()
    {
        yield return new WaitForSeconds(0.9f);
        LevelPanelManager.Instance.Show();
        MapManager.Instance.ClearLevel();
        yield break;
    }
    public void BackToMenu()
    {
        InputManager.Enabled = false;
        LevelPanelManager.Instance.Show();
        MapManager.Instance.ClearLevel();
    }

    private static GameManager instance;

    private bool isLevelComplete;

    private MapInfo enteredLevel;

    private bool levelContainKey;

    private Point keyPoint;
}
