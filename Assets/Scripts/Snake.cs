using UnityEngine;

public class Snake : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject snakePart;
    SnakeMovement SnakeMovement;
    void Start()
    {
        GameObject firstPart = Instantiate(snakePart);
        firstPart.transform.SetParent(transform);
        firstPart.transform.localPosition = new Vector3(0, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(1 * Time.deltaTime, 0, 0);
    }
}
