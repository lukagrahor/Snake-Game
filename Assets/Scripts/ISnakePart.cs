using System.Collections.Generic;
using UnityEngine;
public interface ISnakePart
{
    public void Move();
    public bool isLast();
    public void setLast();
    public void unsetLast();
    public Transform getTransform();
    public float GetRotation();
    public LinkedList<float> GetRotationBuffer();
    public LinkedList<Vector3> GetPositionBuffer();
}
