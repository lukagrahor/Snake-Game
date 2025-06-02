using System;
using System.Drawing;
using UnityEngine;

public class Food : MonoBehaviour, IPickup, ISnakeHeadTriggerHandler, ISpawnableObject, IFlyFrontTriggerHandler, IBiteTriggerHandler
{
    [SerializeField] float size = 0.5f;
    FoodSpawner spawner;
    GridObject locationObject;
    public GridObject LocationObject { get => locationObject; set => locationObject = value; }
    private void OnEnable()
    {
    }

    void EatenByPlayer(SnakeHead snakeHead)
    {
        snakeHead.Grow();
        Use();
        FoodActions.EatenByPlayer?.Invoke();
    }

    public void HandleTrigger(SnakeHead snakeHead)
    {
        EatenByPlayer(snakeHead);
    }

    public void HandleBiteTrigger(SnakeHead snakeHead)
    {
        EatenByPlayer(snakeHead);
    }

    public void Use() {
        gameObject.SetActive(false);
        spawner.RemovePreviousObject(locationObject);
        spawner.Spawn();
    }

    public void SetSpawner(ObjectSpawner spawner)
    {
        this.spawner = (FoodSpawner)spawner;
    }

    public void SetNewPosition(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void ApplyScale()
    {
        gameObject.transform.localScale = new Vector3(size, size, size);
    }

    public void HandleFlyTrigger(Fly fly)
    {
        Use();
        FlyStateMachine stateMachine = (FlyStateMachine) fly.ai.stateMachine;
        if (stateMachine == null) return;
        if (stateMachine.CurrentState == stateMachine.pursueState)
        {
            fly.ai.stateMachine.TransitionTo(stateMachine.idleState);
        }
    }
}
