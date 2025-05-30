using UnityEngine;
using UnityEngine.InputSystem;

public class MobileInputManager : ISnakeInput
{
    InputSystem_Actions _controls;
    Snake snake;
    float minimumSwipeMagnitude = 10f;
    private Vector2 swipeDirection;
    bool waitNextSwipe = false;

    // mislm da je tle problem za delayed input na telefoni
    public MobileInputManager (Snake snake)
    {
        this.snake = snake;
        _controls = new InputSystem_Actions();
        _controls.PlayerMobile.Enable();
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
        _controls.PlayerMobile.Touch.canceled -= TouchCompleted;
        _controls.PlayerMobile.Swipe.performed -= SwipePerformed;
    }

    public void OnSnakeRespawn()
    {
        SubscribeToInput();
    }

    public void SubscribeToInput()
    {
        _controls.PlayerMobile.Touch.canceled += TouchCompleted;
        _controls.PlayerMobile.Swipe.performed += SwipePerformed;
    }

    private void SwipePerformed(InputAction.CallbackContext context)
    {
        if (waitNextSwipe == true)
        {
            return;
        }
        swipeDirection = context.ReadValue<Vector2>();
        if (swipeDirection.magnitude < minimumSwipeMagnitude)
        {
            return;
        }

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
        waitNextSwipe = true;
    }

    void TouchCompleted(InputAction.CallbackContext context)
    {
        waitNextSwipe = false;
    }
}
