using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Camera camera;
    [SerializeField] float speed = 5f;
    Vector3 cameraStartPosition;
    Vector3 destination;
    float startTime;
    float journeyLength;
    bool moveAway = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAway) Move();
    }

    public void MoveCameraAway()
    {
        cameraStartPosition = camera.transform.position;
        startTime = Time.time;

        destination = new Vector3(cameraStartPosition.x, 20f, cameraStartPosition.z);
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        moveAway = true;
    }

    public void Move()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(cameraStartPosition, destination, fractionOfJourney);
        if (fractionOfJourney >= 1) moveAway = false;
    }
}
