using System.Collections.Generic;
using UnityEngine;

public class SnakePath : MonoBehaviour
{
    [SerializeField] SnakePathMarker pathMarkerPrefab1;
    [SerializeField] SnakePathMarker pathMarkerPrefab2;
    SnakePathMarker pathMarkerPrefab;
    List<SnakePathMarker> Path { get; set; }

    void Start()
    {
        pathMarkerPrefab = pathMarkerPrefab2;
        Path = new List<SnakePathMarker>();
    }
    /*
    public void SpawnMarkers(List<GridObject> path)
    {
        //if (pathMarkers != null) RemoveMarkers();

        if (pathMarkerPrefab == pathMarkerPrefab2) pathMarkerPrefab = pathMarkerPrefab1;
        else if (pathMarkerPrefab == pathMarkerPrefab1) pathMarkerPrefab = pathMarkerPrefab2;

        //pathMarkers = new List<GameObject>();

        if (pathMarkerPrefab1 == null || pathMarkerPrefab2 == null)
        {
            Debug.LogError("Path Marker Prefab is not assigned in the Inspector!");
            return;
        }

        foreach (GridObject gridBlock in path)
        {
            Vector3 position = new(gridBlock.transform.position.x, 0.25f, gridBlock.transform.position.z);
            SnakePathMarker pathMarker = Instantiate(pathMarkerPrefab, position, Quaternion.identity);
            pathMarker.transform.parent = transform;
            Path.Add(pathMarker);
        }
    }
    */
    public void SpawnMarker(Vector3 position, float nextRotation)
    {
        //if (pathMarkers != null) RemoveMarkers();

        if (pathMarkerPrefab == pathMarkerPrefab2) pathMarkerPrefab = pathMarkerPrefab1;
        else if (pathMarkerPrefab == pathMarkerPrefab1) pathMarkerPrefab = pathMarkerPrefab2;

        //pathMarkers = new List<GameObject>();

        if (pathMarkerPrefab1 == null || pathMarkerPrefab2 == null)
        {
            Debug.LogError("Path Marker Prefab is not assigned in the Inspector!");
            return;
        }

        Vector3 location = new(position.x, 0.25f, position.z);
        SnakePathMarker pathMarker = Instantiate(pathMarkerPrefab, location, Quaternion.identity);
        pathMarker.transform.parent = transform;
        pathMarker.NextRotation = nextRotation;
        Path.Add(pathMarker);
    }

    public void RemoveMarkers()
    {
        if (Path == null) return;
        foreach (SnakePathMarker pathMarker in Path)
        {
            Destroy(pathMarker);
        }
    }
    //[SerializeField] PathSpawner pathSpawner;

    public void AddToPath(Vector3 location, float nextRotation)
    {
        //Path.Add();
        //pathSpawner.SpawnMarker(location);
    }
}