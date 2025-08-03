using System.Collections.Generic;
using UnityEngine;
public interface ISnakePart : IGridObjectTriggerHandler
{
    public void Move();
    public void UnsetLast();
    public Transform GetTransform();
    public float GetRotation(); 
    public LinkedList<float> GetRotationBuffer();
    public LinkedList<Vector3> GetPositionBuffer();
}
