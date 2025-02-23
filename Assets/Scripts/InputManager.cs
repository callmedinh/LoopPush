using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (!InputManager.Enabled)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerController.Instance.Move(Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerController.Instance.Move(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            PlayerController.Instance.Move(Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            PlayerController.Instance.Move(Direction.Down);
        }
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayerController.Instance.SkipTurn();
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    GameManager.Instance.RestartLevel();
        //}
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    GameManager.Instance.BackToMenu();
        //}
    }
    public static bool Enabled;
}
