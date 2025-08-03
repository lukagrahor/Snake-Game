using UnityEngine;

public class SnakeCorner : MonoBehaviour
{
    Transform parentTransform;

    public void Setup(Transform parentTransform)
    {
        // sledi star�u
        transform.SetParent(parentTransform);
        transform.localPosition = new Vector3(0, 0, 0);
        // ne sledi star�u
        transform.SetParent(null);
        this.parentTransform = parentTransform;
    }

    public void AttachToParent() {
        transform.SetParent(parentTransform);
    }
}
