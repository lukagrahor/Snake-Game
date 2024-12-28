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
        Debug.Log("Barje");
        snake = gameObject.GetComponent<Snake>();
        _controls = new InputSystem_Actions();
        _controls.Player.Enable();
        _controls.Player.Touch.canceled += TouchCompleted;
        _controls.Player.Swipe.performed += SwipePerformed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TouchCompleted(InputAction.CallbackContext context)
    {
        //if (swipeDirection.magnitude < minimumSwipeMagnitude) { return; }
        float snakeYRotation = snake.GetYRotation();
        Debug.Log($"snakeYRotation1: {snakeYRotation}");
        snakeYRotation = snakeYRotation % 360; // to get which direction the snake faces
        if (snakeYRotation < 0)
        {
            snakeYRotation = 360 + snakeYRotation;
        }
        Debug.Log($"snakeYRotation2: {snakeYRotation}");
        Debug.Log($"swipeDirection: {swipeDirection}");
        if (snakeYRotation == 0)
        {
            // Gor desno
            if (swipeDirection.x > 0 && swipeDirection.y > 0)
            {
                snake.SetYRotation(90f);
                //snake.transform.Rotate(0, -90, 0);
            }
            // Dol levo
            else if (swipeDirection.x < 0 && swipeDirection.y < 0)
            {
                snake.SetYRotation(-90f);
                //snake.transform.Rotate(0, 90, 0);
            }
        }

        if (snakeYRotation == 180)
        {
            // Gor desno
            if (swipeDirection.x > 0 && swipeDirection.y > 0)
            {
                //snake.transform.Rotate(0, 90, 0);
                snake.SetYRotation(-90f);
            }
            // Dol levo
            else if (swipeDirection.x < 0 && swipeDirection.y < 0)
            {
                //snake.transform.Rotate(0, -90, 0);
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
        /*
        if (moveDirection.x != 0)
        {
            // Dol desno
            if (swipeDirection.x > 0 && swipeDirection.y < 0)
            {
                snake.SetDirection(new Vector3(0, 0, -1f));
            }
            // Gor levo
            else if (swipeDirection.x < 0 && swipeDirection.y > 0)
            {
                snake.SetDirection(new Vector3(0, 0, 1f));
            }
        }
        else if (moveDirection.z != 0)
        {
            // Dol levo
            if (swipeDirection.x > 0 && swipeDirection.y > 0)
            {
                snake.SetDirection(new Vector3(1f, 0, 0));
            }
            // Gor desno
            else if (swipeDirection.x < 0 && swipeDirection.y < 0)
            {
                snake.SetDirection(new Vector3(-1f, 0, 0));
            }
        }
        */
    }
    private void SwipePerformed(InputAction.CallbackContext context)
    { 
        swipeDirection = context.ReadValue<Vector2>();
    }
}
