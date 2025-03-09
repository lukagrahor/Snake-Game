using UnityEngine;

public class SnakeCorner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Transform parentTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
