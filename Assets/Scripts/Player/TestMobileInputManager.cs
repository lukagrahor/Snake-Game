using UnityEngine;
using UnityEngine.InputSystem;

public class TestMobileInputManager : ISnakeInput
{
    InputSystem_Actions _controls;
    Snake snake;
    //[SerializeField] float minimumSwipeMagnitude = 10f;
    private Vector2 swipeDirection;

    // mislm da je tle problem za delayed input na telefoni
    public TestMobileInputManager(Snake snake)
    {
        this.snake = snake;
        _controls = new InputSystem_Actions();
        _controls.PlayerMobileTest.Enable();
        SubscribeToInput();
    }

    enum MoveDirection
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }

    public void OnSnakeDeath()
    {
        _controls.PlayerMobileTest.Touch.performed -= TouchPerformed;
    }

    public void OnSnakeRespawn()
    {
        SubscribeToInput();
    }

    public void SubscribeToInput()
    {
        _controls.PlayerMobileTest.Touch.performed += TouchPerformed;
    }

    private void TouchPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Touch performed");
        snake.SetNextYRotation(90f);
        /*
        float snakeYRotation = snake.GetSnakeYRotation();
        float nextSnakeYRotation = snake.GetNextHeadRotation();
        float turnLeft = -90f;
        float turnRight = 90f;

        if (snakeYRotation == (float)MoveDirection.Up || nextSnakeYRotation == (float)MoveDirection.Up)
        {
            // Gor desno
            if (swipeDirection.x > 0 && swipeDirection.y > 0)
            {
                snake.SetNextYRotation(turnRight);
            }
            // Dol levo
            else if (swipeDirection.x < 0 && swipeDirection.y < 0)
            {
                snake.SetNextYRotation(turnLeft);
            }
        }

        if (snakeYRotation == (float)MoveDirection.Down || nextSnakeYRotation == (float)MoveDirection.Down)
        {
            // Gor desno
            if (swipeDirection.x > 0 && swipeDirection.y > 0)
            {
                snake.SetNextYRotation(turnLeft);
            }
            // Dol levo
            else if (swipeDirection.x < 0 && swipeDirection.y < 0)
            {
                snake.SetNextYRotation(turnRight);
            }
        }

        if (snakeYRotation == (float)MoveDirection.Right || nextSnakeYRotation == (float)MoveDirection.Right)
        {
            // Dol desno
            if (swipeDirection.x > 0 && swipeDirection.y < 0)
            {
                snake.SetNextYRotation(turnRight);
            }
            // Gor levo
            else if (swipeDirection.x < 0 && swipeDirection.y > 0)
            {
                snake.SetNextYRotation(turnLeft);
            }
        }

        if (snakeYRotation == (float)MoveDirection.Left || nextSnakeYRotation == (float)MoveDirection.Left)
        {
            // Dol desno
            if (swipeDirection.x > 0 && swipeDirection.y < 0)
            {
                snake.SetNextYRotation(turnLeft);
            }
            // Gor levo
            else if (swipeDirection.x < 0 && swipeDirection.y > 0)
            {
                snake.SetNextYRotation(turnRight);
            }
        }
        */
    }
    private void SwipePerformed(InputAction.CallbackContext context)
    {
        swipeDirection = context.ReadValue<Vector2>();
    }
}
