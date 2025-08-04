using System.Collections.Generic;
using UnityEngine;

public class PrimaryCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameManager gameManager;
    [SerializeField] PrimaryCamera cam;
    [SerializeField] MarketCamera marketCamera;
    [SerializeField] float speed = 5f;
    [SerializeField] List<CornerBlock> cornerBlocks;
    [SerializeField] Arena arena;
    Vector3 cameraDefaultPosition;
    Vector3 cameraStartPosition;
    Vector3 destination;
    float startTime;
    float journeyLength;
    bool move = false;
    bool movingAway = false;

    CountDown timer;
    float prepareTime = 1.5f;
    void Start()
    {
        timer = new CountDown(prepareTime);
        timer.TimeRanOut += ChangeCamera;
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
        cameraStartPosition.y = cornerBlocks[0].transform.position.y;
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
        cameraStartPosition.y = cornerBlocks[0].transform.position.y;
        startTime = Time.time;

        destination = cameraDefaultPosition;
        journeyLength = Vector3.Distance(cameraStartPosition, destination);
        //EnableCornerBlocks();
        gameManager.SpawnPlayerAndEnemies();
        move = true;
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
        //timer.Timer = prepareTime;
        //timer.Start();
        gameManager.StartNewLevel();
    }

    public void Move()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;
        foreach (CornerBlock corner in cornerBlocks)
        {
            Vector3 cornerPosition = corner.transform.position;
            Vector3 blockStartPosition = new Vector3(cornerPosition.x, cameraStartPosition.y, cornerPosition.z);
            destination.x = cornerPosition.x;
            destination.z = cornerPosition.z;
            corner.transform.position = Vector3.Lerp(blockStartPosition, destination, fractionOfJourney);
        }
        if (fractionOfJourney >= 1)
        {
            //Debug.Log("Reached destination");
            move = false;

            timer.Timer = prepareTime;
            timer.Start();

            // without this, when the camera comes back to the arena it moves away again and repeats that
            if (movingAway)
            {
                Prepare();
                movingAway = false;
            } else
            {
                arena.SetCamera();
            }
        }
    }

    void ChangeCamera()
    {
        Debug.Log("Change camera");
        cam.gameObject.SetActive(false);
        marketCamera.gameObject.SetActive(true);
        marketCamera.StartMoving(marketCamera.transform.position, new Vector3(-2.468f, 6.408f, -2.98f));
    }
}
