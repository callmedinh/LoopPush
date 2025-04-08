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
    }
    private void OnEnable()
    {
        if (DeData.Instance == null)
        {
            Debug.LogError("DeData Instance is NULL! Ensure DeData is in the scene.");
            return;
        }

        DeData.Instance.Initialize();
        for (int i = 0; i < this.levelButtonList.Count; i++)
        {
            int chapter = i / 4 + 1;
            int level = i % 4;
            levelButtonList[i].interactable = DeData.Instance.GetLevelData(chapter, level).CanStart;
            this.levelImageList[i].sprite = (DeData.Instance.GetLevelData(chapter, level).CanStart ? this.openedSprite : this.lockedSprite);
            this.levelImageList[i].color = (DeData.Instance.GetLevelData(chapter, level).Passed ? this.passedColor : Color.white);
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
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
        GamePanelManager.Instance.Show();
        GameManager.Instance.EnterLevel(level);
        this.enableClick = true;
    }

    private static LevelPanelManager instance;
    [SerializeField]
    private Sprite openedSprite; 
    [SerializeField]
    private Sprite lockedSprite;
    [SerializeField]
    private Transform levelGrid;
    public List<Image> levelImageList;
    public List<Button> levelButtonList;
    [SerializeField]
    private Color passedColor;

    private bool enableClick = true;
}
