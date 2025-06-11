using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameManager gameManager;
    [SerializeField] Camera camera;
    [SerializeField] float speed = 5f;
    Vector3 cameraDefaultPosition;
    Vector3 cameraStartPosition;
    Vector3 destination;
    float startTime;
    float journeyLength;
    bool move = false;

    CountDown timer;
    float prepareTime = 2f;
    void Start()
    {
        timer = new CountDown(prepareTime);
        timer.TimeRanOut += MoveCameraBack;
    }

    // Update is called once per frame
    void Update()
    {
        if (move) Move();
        timer?.Update();
    }

    public void MoveCameraAway()
    {
        Debug.Log("Hodi èja");
        cameraStartPosition = camera.transform.position;
        cameraDefaultPosition = cameraStartPosition;
        startTime = Time.time;

        
        destination = new Vector3(cameraStartPosition.x, 8f, cameraStartPosition.z);
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        move = true;
    }

    void MoveCameraBack()
    {
        cameraStartPosition = camera.transform.position;
        startTime = Time.time;

        destination = cameraDefaultPosition;
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        move = true;
    }

    void Prepare()
    {
        timer.Timer = prepareTime;
        timer.Start();
        gameManager.StartNewLevel();
    }

    public void Move()
    {
        Debug.Log("moooooove");
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(cameraStartPosition, destination, fractionOfJourney);
        if (fractionOfJourney >= 1)
        {
            Debug.Log("Reached destination");
            move = false;
            Prepare();
        }
    }
}
