using UnityEngine;

public class SnakeCorner : MonoBehaviour
{
    Transform parentTransform;

    public void Setup(Transform parentTransform)
    {
        // sledi staršu
        transform.SetParent(parentTransform);
        transform.localPosition = new Vector3(0, 0, 0);
        // ne sledi staršu
        transform.SetParent(null);
        this.parentTransform = parentTransform;
    }

    public void AttachToParent() {
        transform.SetParent(parentTransform);
    }
}
