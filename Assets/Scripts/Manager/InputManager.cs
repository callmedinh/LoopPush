using Controller;
using Events;
using UnityEngine;

namespace Manager
{
    public class InputManager : MonoBehaviour
    {
        public static bool Enabled;

        private void Update()
        {
            if (!Enabled) return;
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                InputEvent.OnLeftDirectionPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                InputEvent.OnRightDirectionPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                InputEvent.OnUpDirectionPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                InputEvent.OnDownDirectionPressed?.Invoke();
            if (Input.GetKeyDown(KeyCode.Space)) PlayerController.Instance.SkipTurn();
            if (Input.GetKeyDown(KeyCode.R)) GameManager.Instance.RestartLevel();
            if (Input.GetKeyDown(KeyCode.Escape)) GameManager.Instance.BackToMenu();
        }
    }
}