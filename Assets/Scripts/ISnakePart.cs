using UnityEngine;
public interface ISnakePart
{
    public void Move();
    public bool isLast();
    public void setLast();
    public void unsetLast();
    public Transform getTransform();
}
