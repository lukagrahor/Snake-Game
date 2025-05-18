using UnityEngine;
using UnityEngine.InputSystem;

public class DesktopInputManager : ISnakeInput
{
    InputSystem_Actions _controls;
    Snake snake;
    float defaultSnakeSpeed = 0f;
    public DesktopInputManager (Snake snake)
    {
        this.snake = snake;
        _controls = new InputSystem_Actions();
        _controls.PlayerDesktop.Enable();
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
        _controls.PlayerDesktop.MoveUp.performed -= MoveUp;
        _controls.PlayerDesktop.MoveRight.performed -= MoveRight;
        _controls.PlayerDesktop.MoveDown.performed -= MoveDown;
        _controls.PlayerDesktop.MoveLeft.performed -= MoveLeft;
    }

    public void OnSnakeRespawn()
    {
        SubscribeToInput();
    }

    public void SubscribeToInput()
    {
        _controls.PlayerDesktop.MoveUp.performed += MoveUp;
        _controls.PlayerDesktop.MoveRight.performed += MoveRight;
        _controls.PlayerDesktop.MoveDown.performed += MoveDown;
        _controls.PlayerDesktop.MoveLeft.performed += MoveLeft;
        _controls.PlayerDesktop.PrimaryAttack.performed += PrimaryAttackPerformed;
        _controls.PlayerDesktop.PrimaryAttack.canceled += PrimaryAttackCanceled;
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

        if (snakeYRotation == (float)MoveDirection.Up || nextSnakeYRotation == (float)MoveDirection.Up)
        {
            snake.SetNextYRotation(turnLeft);
        }
        else if (snakeYRotation == (float)MoveDirection.Down || nextSnakeYRotation == (float)MoveDirection.Down)
        {
            snake.SetNextYRotation(turnRight);
        }
    }

    private void PrimaryAttackPerformed(InputAction.CallbackContext context)
    {
        snake.MoveSpeed = snake.ChargingMoveSpeed;
        snake.SnakeHead.SetBiteMovementDirection();
        snake.IsBiting = true;
        // onemogoèi spreminjanje smeri cele kaèe - le od glave
    }

    private void PrimaryAttackCanceled(InputAction.CallbackContext context)
    {
        // preveri ali bite timer potekel --> èe je ne naredi niè
        snake.MoveSpeed = snake.DefaultSpeed;
        snake.IsBiting = false;
        snake.SnakeHead.StopBiting();
        // nastavi smer na shranjeno smer

        // izvedi ugriz --> ugrizni pred kaèo
        // omogoèi nazaj spreminjanje smeri --> al daj nazaj na prejšnjo smer al pa upoštevaj smer po ugrizu
    }

    /*
    private void PrimaryAttack(InputAction.CallbackContext context)
    {
        float snakeSpeed = snake.MoveSpeed;
        // To ustavi kaèo
        if (snakeSpeed != 0f)
        {
            defaultSnakeSpeed = snakeSpeed;
            snake.MoveSpeed = 0f;
            return;
        }

        if (defaultSnakeSpeed == 0f)
        {
            Debug.Log("Ups, defaultSnakeSpeed je niè");
            return;
        }

        snake.MoveSpeed = defaultSnakeSpeed;
    }
    */
}
