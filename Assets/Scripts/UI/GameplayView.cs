using System.Collections.Generic;
using Controller;
using DG.Tweening;
using Events;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class GameplayView : UIBaseView
    {
        private int nodeCount;
        private int currentNode;
        private List<NodeController> nodeObjList = new List<NodeController>();

        [SerializeField] private Image circleImage;
        [SerializeField] private Image bridgeImage;
        [SerializeField] private Image nodeCircleImage;
        [SerializeField] private Transform mainTrackParent;
        [SerializeField] private Transform timelineParent;
        
        //Direction Button
        [SerializeField] private Button leftDirButton;
        [SerializeField] private Button rightDirButton;
        [SerializeField] private Button upDirButton;
        [SerializeField] private Button downDirButton;
        [SerializeField] private Button settingButton;

        private void OnDisable()
        {
            InputEvent.OnLeftDirectionPressed -= () => leftDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnRightDirectionPressed -= () => rightDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnUpDirectionPressed -= () => upDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnDownDirectionPressed -= () => downDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);

            GameplayEvent.OnPlayerStepTaken -= StepOneNode;
            GameplayEvent.OnLoopEnded -= ClearNodeStep;
            ClearAllTimeline();
        }

        private void OnEnable()
        {
            InitTimelineNode(MapManager.CurrentMap.steps);
            leftDirButton.onClick.AddListener(() => InputEvent.OnLeftDirectionPressed?.Invoke());
            rightDirButton.onClick.AddListener(() => InputEvent.OnRightDirectionPressed?.Invoke());
            upDirButton.onClick.AddListener(() => InputEvent.OnUpDirectionPressed?.Invoke());
            downDirButton.onClick.AddListener(() => InputEvent.OnDownDirectionPressed?.Invoke());
            
            InputEvent.OnLeftDirectionPressed += () => leftDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnRightDirectionPressed += () => rightDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnUpDirectionPressed += () => upDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            InputEvent.OnDownDirectionPressed += () => downDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);

            GameplayEvent.OnPlayerStepTaken += StepOneNode;
            GameplayEvent.OnLoopEnded += ClearNodeStep;
        }
        private void Update()
        {
            foreach (var node in nodeObjList)
            {
                node.Update();
            }
        }

        public void InitTimelineNode(int count)
        {
            ClearAllTimeline();
            nodeCount = count;
            currentNode = 0;
            for (int i = 0; i <= count; i++)
            {
                if (i == 0)
                {
                    Instantiate(circleImage, timelineParent);   
                    Instantiate(nodeCircleImage, mainTrackParent);
                }
                else
                {
                    Instantiate(bridgeImage, timelineParent);
                    Instantiate(circleImage, timelineParent);

                    Image bridge = Instantiate(bridgeImage, mainTrackParent);
                    Image circle = Instantiate(nodeCircleImage, mainTrackParent);
                    NodeController node = new NodeController(bridge, circle);
                    node.ClearFill();
                    nodeObjList.Add(node);
                }
            }
        }
        public void StepOneNode()
        {
            if (currentNode < nodeCount)
            {
                nodeObjList[this.currentNode].StartFill();
                currentNode++;   
            }
        }
        public void ClearNodeStep()
        {
            for (int i = 0; i < this.nodeObjList.Count; i++)
            {
                nodeObjList[i].ClearFill();
            }
            currentNode = 0;
        }
        private void ClearAllTimeline()
        {
            foreach (Transform child in timelineParent)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform child in mainTrackParent)
            {
                Destroy(child.gameObject);
            }
            nodeObjList.Clear();
            currentNode = 0;
        }
    }
}