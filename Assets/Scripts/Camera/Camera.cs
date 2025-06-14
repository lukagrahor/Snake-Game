using UnityEngine;

public class PrimaryCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameManager gameManager;
    [SerializeField] PrimaryCamera cam;
    [SerializeField] float speed = 5f;
    Vector3 cameraDefaultPosition;
    Vector3 cameraStartPosition;
    Vector3 destination;
    float startTime;
    float journeyLength;
    bool move = false;
    bool movingAway = false;

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
        //Debug.Log("Odmokni kamero!");
        cameraStartPosition = cam.transform.position;
        cameraDefaultPosition = cameraStartPosition;
        startTime = Time.time;

        
        destination = new Vector3(cameraStartPosition.x, 8f, cameraStartPosition.z);
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        move = true;
        movingAway = true;
    }

    void MoveCameraBack()
    {
        //Debug.Log("Ajmo nazaj!");
        cameraStartPosition = cam.transform.position;
        startTime = Time.time;

        destination = cameraDefaultPosition;
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        EnableCornerBlocks();
        //move = true;
    }

    void EnableCornerBlocks()
    {
        CornerBlock[] blocks = FindObjectsByType<CornerBlock>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CornerBlock block in blocks)
        {
            block.gameObject.SetActive(true);
        }
    }

    void Prepare()
    {
        //Debug.Log("Pripravi se na premik!");
        timer.Timer = prepareTime;
        timer.Start();
        gameManager.StartNewLevel();
    }

    public void Move()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(cameraStartPosition, destination, fractionOfJourney);
        if (fractionOfJourney >= 1)
        {
            //Debug.Log("Reached destination");
            move = false;
            // without this, when the camera comes backt to the arena it moves away again and repeats that
            if (movingAway)
            {
                Prepare();
                movingAway = false;
            }
        }
    }
}
