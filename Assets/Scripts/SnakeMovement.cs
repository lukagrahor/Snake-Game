using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SnakeMovement : MonoBehaviour
{
    InputSystem_Actions _controls;
    [SerializeField] float minimumSwipeMagnitude = 10f;
    Snake snake;
    private Vector2 swipeDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Debug.Log("Barje");
        snake = gameObject.GetComponent<Snake>();
        _controls = new InputSystem_Actions();
        _controls.Player.Enable();
        _controls.Player.Touch.canceled += TouchCompleted;
        _controls.Player.Swipe.performed += SwipePerformed;
        _controls.Player.MoveLeft.performed += MoveLeft;
        _controls.Player.MoveRight.performed += MoveRight;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TouchCompleted(InputAction.CallbackContext context)
    {
        //if (swipeDirection.magnitude < minimumSwipeMagnitude) { return; }
        float snakeYRotation = snake.GetAbsoluteRotation();
        //Debug.Log("Huh1");
        if (swipeDirection.x > 0)
        {
            snake.SetNextYRotation(90f);
        }
        else if (swipeDirection.x < 0)
        {
            snake.SetNextYRotation(-90f);
        }
            /*
            if (snakeYRotation == 0)
            {
                // Gor desno
                if (swipeDirection.x > 0 && swipeDirection.y > 0)
                {
                    snake.SetYRotation(90f);
                }
                // Dol levo
                else if (swipeDirection.x < 0 && swipeDirection.y < 0)
                {
                    snake.SetYRotation(-90f);
                }
            }

            if (snakeYRotation == 180)
            {
                // Gor desno
                if (swipeDirection.x > 0 && swipeDirection.y > 0)
                {
                    snake.SetYRotation(-90f);
                }
                // Dol levo
                else if (swipeDirection.x < 0 && swipeDirection.y < 0)
                {
                    snake.SetYRotation(90f);
                }
            }

            if (snakeYRotation == 90)
            {
                // Dol desno
                if (swipeDirection.x > 0 && swipeDirection.y < 0)
                {
                    snake.SetYRotation(90f);
                }
                // Gor levo
                else if (swipeDirection.x < 0 && swipeDirection.y > 0)
                {
                    snake.SetYRotation(-90f);
                }
            }

            if (snakeYRotation == 270)
            {
                // Dol desno
                if (swipeDirection.x > 0 && swipeDirection.y < 0)
                {
                    snake.SetYRotation(-90f);
                }
                // Gor levo
                else if (swipeDirection.x < 0 && swipeDirection.y > 0)
                {
                    snake.SetYRotation(90f);
                }
            }
            */
        }
    private void SwipePerformed(InputAction.CallbackContext context)
    { 
        swipeDirection = context.ReadValue<Vector2>();
    }
    private void MoveLeft(InputAction.CallbackContext context)
    {
        snake.SetNextYRotation(-90f);
        //Debug.Log("Huh2");
    }
    private void MoveRight(InputAction.CallbackContext context)
    {
        snake.SetNextYRotation(90f);
        //Debug.Log("Huh3");
    }
}
