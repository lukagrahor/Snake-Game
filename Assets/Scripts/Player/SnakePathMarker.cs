using UnityEngine;

public class SnakePathMarker : MonoBehaviour
{
    float nextRotation;
    public float NextRotation { get => nextRotation; set => nextRotation = value; }

    private void OnTriggerEnter(Collider other)
    {
        IPathTrigger obj = other.GetComponent<IPathTrigger>();
        obj?.HandlePathTrigger(this);
    }

    private void Start()
    {
        if (nextRotation > 0f) Debug.Log("NextRotation marker " + this.name + " rotation " + nextRotation);
    }
}