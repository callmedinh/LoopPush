using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPanelManager : MonoBehaviour
{
    public static LevelPanelManager Instance
    {
        get
        {
            return LevelPanelManager.instance;
        }
    }

    private void Awake()
    {
        LevelPanelManager.instance = this;
        //for (int i = 1; i < 4; i++)
        //{
        //    for (int j = 0; j < 4; j++)
        //    {
        //        Transform transform = this.levelGrid.Find(i.ToString() + j.ToString());
        //        Image component = transform.GetComponent<Image>();
        //        Button component2 = transform.GetComponent<Button>();
        //        this.levelImageList.Add(component);
        //        this.levelButtonList.Add(component2);
        //    }
        //}
        //this.Hide();
    }

    //private void OnEnable()
    //{
    //    Debug.Log("On Level Enable");
    //    for (int i = 0; i < this.levelImageList.Count; i++)
    //    {
    //        int chapter = i / 4 + 1;
    //        int level = i % 4;
    //        this.levelImageList[i].sprite = (DeData.Instance.GetLevelData(chapter, level).CanStart ? this.openedSprite : this.lockedSprite);
    //        this.levelImageList[i].color = (DeData.Instance.GetLevelData(chapter, level).Passed ? this.passedColor : Color.white);
    //        this.levelButtonList[i].interactable = DeData.Instance.GetLevelData(chapter, level).CanStart;
    //    }
    //}

    //private IEnumerator StartInputListen()
    //{
    //    while (!Input.GetKeyDown(KeyCode.Space))
    //    {
    //        yield return null;
    //    }
    //    FadeTransitionManager.Instance.Fade(new Action(this.Hide));
    //    yield break;
    //}

    public void Show()
    {
        base.gameObject.SetActive(true);
    }

    public void Hide()
    {
        base.gameObject.SetActive(false);
    }

    public void OnLevelClick(string level)
    {
        if (!this.enableClick)
        {
            return;
        }
        this.enableClick = false;
        this.Hide();
        GameManager.Instance.EnterLevel(level);
        GamePanelManager.Instance.Show();
        this.enableClick = true;
    }

    private static LevelPanelManager instance;

    [SerializeField]
    private Sprite lockedSprite;

    [SerializeField]
    private Sprite openedSprite;

    [SerializeField]
    private Color passedColor;

    [SerializeField]
    private Transform levelGrid;

    private List<Image> levelImageList = new List<Image>();

    private List<Button> levelButtonList = new List<Button>();

    private bool enableClick = true;
}
