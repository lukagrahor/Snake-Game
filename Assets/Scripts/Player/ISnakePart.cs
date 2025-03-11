using System.Collections.Generic;
using UnityEngine;
public interface ISnakePart
{
    public void Move();
    public bool IsLast();
    public void SetLast();
    public void UnsetLast();
    public Transform GetTransform();
    public float GetRotation();
    public LinkedList<float> GetRotationBuffer();
    public LinkedList<Vector3> GetPositionBuffer();
}
