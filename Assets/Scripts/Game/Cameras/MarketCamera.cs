using UnityEngine;

public class MarketCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float speed = 6f;
    Vector3 startPosition;
    Vector3 goalPosition;
    private float startTime;
    private float journeyLength;

    bool isMoving = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) Move();
    }

    public void StartMoving(Vector3 start, Vector3 goal)
    {
        startTime = Time.time;
        startPosition = start;
        goalPosition = goal;
        journeyLength = Vector3.Distance(startPosition, goalPosition);
        isMoving = true;
    }

    void Move()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, goalPosition, fractionOfJourney);

        if (fractionOfJourney >= 1) isMoving = false;
    }
}
