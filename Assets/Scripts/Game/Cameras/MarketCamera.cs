using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MarketCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Canvas marketCanvas;
    [SerializeField] Button backButton;
    [SerializeField] PrimaryCamera mainCamera;
    [SerializeField] Market market;
    float speed = 6f;
    Vector3 startPosition;
    Vector3 goalPosition;
    private float startTime;
    private float journeyLength;

    bool isMovingBack = false;

    bool isMoving = false;
    private void OnEnable()
    {
        isMovingBack = false;
    }
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

        marketCanvas.gameObject.SetActive(true);
        backButton.interactable = false;

        market.SpawnNewItems();
    }

    public void StartMovingBackwards()
    {
        startTime = Time.time;
        startPosition = transform.position;
        goalPosition = new Vector3(-4.26f, 6.92f, -5.12f);
        journeyLength = Vector3.Distance(startPosition, goalPosition);
        isMoving = true;
        backButton.interactable = false;
        isMovingBack = true;
    }

    void Move()
    {
        float distCovered = (Time.time - startTime) * speed;

        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(startPosition, goalPosition, fractionOfJourney);

        if (fractionOfJourney >= 1)
        {
            isMoving = false;
            backButton.interactable = true;
            if (isMovingBack)
            {
                mainCamera.gameObject.SetActive(true);
                mainCamera.MoveCameraBack();
                marketCanvas.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
