using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SnakeMovement : MonoBehaviour
{
    InputSystem_Actions _controls;
    [SerializeField] float minimumSwipeMagnitude = 10f;
    Snake snake;
    private Vector2 swipeDirection;
    enum MoveDirection
    {
        Up = 0,
        Right = 90,
        Down = 180,
        Left = 270
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Barje");
        snake = gameObject.GetComponent<Snake>();
        _controls = new InputSystem_Actions();
        _controls.Player.Enable();
        _controls.Player.Touch.canceled += TouchCompleted;
        _controls.Player.Swipe.performed += SwipePerformed;
        _controls.Player.MoveUp.performed += MoveUp;
        _controls.Player.MoveRight.performed += MoveRight;
        _controls.Player.MoveDown.performed += MoveDown;
        _controls.Player.MoveLeft.performed += MoveLeft;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnSnakeDeath()
    {
        _controls.Player.Touch.canceled -= TouchCompleted;
        _controls.Player.Swipe.performed -= SwipePerformed;
        _controls.Player.MoveUp.performed -= MoveUp;
        _controls.Player.MoveRight.performed -= MoveRight;
        _controls.Player.MoveDown.performed -= MoveDown;
        _controls.Player.MoveLeft.performed -= MoveLeft;
    }

    public void OnSnakeRespawn()
    {
        _controls.Player.Touch.canceled += TouchCompleted;
        _controls.Player.Swipe.performed += SwipePerformed;
        _controls.Player.MoveUp.performed += MoveUp;
        _controls.Player.MoveRight.performed += MoveRight;
        _controls.Player.MoveDown.performed += MoveDown;
        _controls.Player.MoveLeft.performed += MoveLeft;
    }

    /*
    private void OnApplicationPause()
    {
        if (_controls != null)
        {
            _controls.Player.Disable();
        }
    }*/

    private void OnApplicationQuit()
    {
        if (_controls != null)
        {
            _controls.Player.Disable();
        }
    }

    private void TouchCompleted(InputAction.CallbackContext context)
    {
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
    }
    private void SwipePerformed(InputAction.CallbackContext context)
    { 
        swipeDirection = context.ReadValue<Vector2>();
    }
    private void MoveUp(InputAction.CallbackContext context)
    {
        float snakeYRotation = snake.GetSnakeYRotation();
        float nextSnakeYRotation = snake.GetNextHeadRotation();
        float turnLeft = -90f;
        float turnRight = 90f;
        if (snakeYRotation == (float)MoveDirection.Right || nextSnakeYRotation == (float)MoveDirection.Right)
        {
            snake.SetNextYRotation(turnLeft);
        }
        else if (snakeYRotation == (float)MoveDirection.Left || nextSnakeYRotation == (float)MoveDirection.Left)
        {
            snake.SetNextYRotation(turnRight);
        }
    }
    private void MoveRight(InputAction.CallbackContext context)
    {
        float snakeYRotation = snake.GetSnakeYRotation();
        float nextSnakeYRotation = snake.GetNextHeadRotation();
        float turnLeft = -90f;
        float turnRight = 90f;
        if (snakeYRotation == (float)MoveDirection.Up || nextSnakeYRotation == (float)MoveDirection.Up)
        {
            Debug.Log("Datarnjan");
            snake.SetNextYRotation(turnRight);
        }
        else if (snakeYRotation == (float)MoveDirection.Down || nextSnakeYRotation == (float)MoveDirection.Down)
        {
            snake.SetNextYRotation(turnLeft);
        }
    }    
    private void MoveDown(InputAction.CallbackContext context)
    {
        float snakeYRotation = snake.GetSnakeYRotation();
        // da se upošteva tudi naslednja pozicija v bufferji --> pri kroženju je bolj responsive
        float nextSnakeYRotation = snake.GetNextHeadRotation();

        float turnLeft = -90f;
        float turnRight = 90f;
        Debug.Log($"MoveDown: {snakeYRotation}");
        Debug.Log($"snakeYRotation: {snakeYRotation}");
        Debug.Log($"nextSnakeYRotation: {nextSnakeYRotation}");
        if (snakeYRotation == (float)MoveDirection.Right || nextSnakeYRotation == (float)MoveDirection.Right)
        {
            snake.SetNextYRotation(turnRight);
        }
        else if (snakeYRotation == (float)MoveDirection.Left || nextSnakeYRotation == (float)MoveDirection.Left)
        {
            snake.SetNextYRotation(turnLeft);
        }
    }
    private void MoveLeft(InputAction.CallbackContext context)
    {
        float snakeYRotation = snake.GetSnakeYRotation();
        float nextSnakeYRotation = snake.GetNextHeadRotation();
        float turnLeft = -90f;
        float turnRight = 90f;
        Debug.Log("--------------------");
        Debug.Log($"MoveLeft: {snakeYRotation}");
        Debug.Log($"snakeYRotation: {snakeYRotation}");
        Debug.Log($"nextSnakeYRotation: {nextSnakeYRotation}");
        Debug.Log("--------------------");
        if (snakeYRotation == (float)MoveDirection.Up || nextSnakeYRotation == (float)MoveDirection.Up)
        {
            snake.SetNextYRotation(turnLeft);
        }
        else if (snakeYRotation == (float)MoveDirection.Down || nextSnakeYRotation == (float)MoveDirection.Down)
        {
            snake.SetNextYRotation(turnRight);
        }
    }
}
