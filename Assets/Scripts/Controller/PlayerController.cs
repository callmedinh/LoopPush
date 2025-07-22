using System.Collections;
using DefaultNamespace;
using Events;
using UnityEngine;
using Utilities;

namespace Controller
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private ShadowMovement shadowMovement;
        [SerializeField] private PlayerMovement playerMovement;
        private LoopSystem loopSystem;
        private StepSystem stepSystem;

        protected override void Awake()
        {
            base.Awake();
            InputEvent.OnDownDirectionPressed += () => Move(Direction.Down);
            InputEvent.OnUpDirectionPressed += () => Move(Direction.Up);
            InputEvent.OnLeftDirectionPressed += () => Move(Direction.Left);
            InputEvent.OnRightDirectionPressed += () => Move(Direction.Right);
        }

        public void Init(Vector2Int position, int steps)
        {
            loopSystem = new LoopSystem(new Direction[steps]);
            LoopInit(position, steps);
        }

        public void LoopInit(Vector2Int position, int steps)
        {
            stepSystem = new StepSystem(steps);
            shadowMovement.Init(position);
            playerMovement.Init(position);
            shadowMovement.gameObject.SetActive(loopSystem.LastLoop != null);
        }

        public void ClearLastLoop()
        {
            loopSystem.ResetLoop();
        }

        private void Move(Direction dir)
        {
            if (!stepSystem.CanMove()) return;
            if (playerMovement.TryMove(dir, out var teleported))
            {
                loopSystem.CurrentLoop[stepSystem.Total - stepSystem.Left] = dir;
                if (loopSystem.HasLastLoop())
                    StartCoroutine(WaitShadowMove(loopSystem.LastLoop[stepSystem.CurrentStepIndex]));
                stepSystem.UseStep();
                if (stepSystem.IsLastStep) StartCoroutine(loopSystem.WaitEndLoop());
            }
        }

        private IEnumerator WaitShadowMove(Direction dir)
        {
            yield return new WaitForSeconds(0.1f);
            shadowMovement.MoveTo(dir);
        }

        public void SkipTurn()
        {
            if (loopSystem.HasLastLoop())
                shadowMovement.MoveTo(loopSystem.LastLoop[stepSystem.Total - stepSystem.Left]);
            loopSystem.SkipStep(stepSystem.CurrentStepIndex);
            stepSystem.UseStep();
            if (stepSystem.IsLastStep) StartCoroutine(loopSystem.WaitEndLoop());
        }
    }
}