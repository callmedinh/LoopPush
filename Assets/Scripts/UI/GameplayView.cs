using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using DG.Tweening;
using Events;
using Manager;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        [SerializeField] private TMP_Text instructionText;
        
        //Direction Button
        [SerializeField] private Button leftDirButton;
        [SerializeField] private Button rightDirButton;
        [SerializeField] private Button upDirButton;
        [SerializeField] private Button downDirButton;
        [SerializeField] private Button skipTurnButton;
        [SerializeField] private Button settingButton;

        public Action LeftEffectHandler;
        public Action RightEffectHandler;
        public Action UpEffectHandler;
        public Action DownEffectHandler;
        public Action SkipTurnEffectHandler;
        
        private Dictionary<string, InstructionSO> instructionMap = new Dictionary<string, InstructionSO>();

        private void Awake()
        {
            LoadInstruction();
        }

        private void OnDisable()
        {
            InputEvent.OnLeftDirectionPressed -= LeftEffectHandler;
            InputEvent.OnRightDirectionPressed -= RightEffectHandler;
            InputEvent.OnUpDirectionPressed -= UpEffectHandler;
            InputEvent.OnDownDirectionPressed -= DownEffectHandler;
            InputEvent.OnSkipTurnButtonPressed -= SkipTurnEffectHandler;
            
            GameplayEvent.OnPlayerStepTaken -= StepOneNode;
            GameplayEvent.OnLoopEnded -= ClearNodeStep;
            GameplayEvent.OnPlayerLevelChanged += OnCurrentLevelChanged;

            StopCoroutine(LoopFontStyle());
            ClearAllTimeline();
        }

        private void OnEnable()
        {
            LeftEffectHandler = () => leftDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            RightEffectHandler = () => rightDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            UpEffectHandler = () => upDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            DownEffectHandler = () => downDirButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            SkipTurnEffectHandler = () => skipTurnButton.transform.DOScale(1.2f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutBounce);
            
            InitTimelineNode(MapManager.CurrentMap.steps);
            StartCoroutine(LoopFontStyle());
            leftDirButton.onClick.AddListener(() => InputEvent.OnLeftDirectionPressed?.Invoke());
            rightDirButton.onClick.AddListener(() => InputEvent.OnRightDirectionPressed?.Invoke());
            upDirButton.onClick.AddListener(() => InputEvent.OnUpDirectionPressed?.Invoke());
            downDirButton.onClick.AddListener(() => InputEvent.OnDownDirectionPressed?.Invoke());
            skipTurnButton.onClick.AddListener(() => InputEvent.OnSkipTurnButtonPressed?.Invoke());

            InputEvent.OnLeftDirectionPressed += LeftEffectHandler;
            InputEvent.OnRightDirectionPressed += RightEffectHandler;
            InputEvent.OnUpDirectionPressed += UpEffectHandler;
            InputEvent.OnDownDirectionPressed += DownEffectHandler;
            InputEvent.OnSkipTurnButtonPressed += SkipTurnEffectHandler;
            
            GameplayEvent.OnPlayerStepTaken += StepOneNode;
            GameplayEvent.OnLoopEnded += ClearNodeStep;
            GameplayEvent.OnPlayerLevelChanged += OnCurrentLevelChanged;
        }
        private void Update()
        {
            foreach (var node in nodeObjList)
            {
                node.Update();
            }
        }
        private void OnCurrentLevelChanged(string levelId)
        {
            if (instructionMap.TryGetValue(levelId, out InstructionSO instruction))
            {
                instructionText.text = instruction.GetInstruction(true);
            }
            else
            {

            }
        }
        public void LoadInstruction()
        {
            // Load the instruction based on the levelId and language preference
            var allInstructions = Resources.LoadAll<ScriptableObjects.InstructionSO>($"Instructions");
            foreach (var inst in allInstructions)
            {
                instructionMap[inst.levelId] = inst;
            }
        }

        private IEnumerator LoopFontStyle()
        {
            while (true)
            {
                // Random style: 0 = Normal, 1 = Bold, 2 = Italic, 3 = BoldItalic
                int styleIndex = Random.Range(0, 4);
                switch (styleIndex)
                {
                    case 0:
                        instructionText.fontStyle = FontStyles.Normal;
                        break;
                    case 1:
                        instructionText.fontStyle = FontStyles.Bold;
                        break;
                    case 2:
                        instructionText.fontStyle = FontStyles.Italic;
                        break;
                    case 3:
                        instructionText.fontStyle = FontStyles.Bold | FontStyles.Italic;
                        break;
                }
                yield return new WaitForSeconds(0.5f); // thời gian giữa các lần thay đổi
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