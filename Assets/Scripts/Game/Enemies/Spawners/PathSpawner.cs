using UnityEngine;
using System.Collections.Generic;
public class PathSpawner : MonoBehaviour
{
    [SerializeField] GameObject pathMarkerPrefab1;
    [SerializeField] GameObject pathMarkerPrefab2;
    GameObject pathMarkerPrefab;
    List<GameObject> pathMarkers;

    void Start()
    {
        pathMarkerPrefab = pathMarkerPrefab2;
        pathMarkers = new List<GameObject>();
    }
    // nekej je narobe. Namesto da samo doda trenutni poti, gre vse znova in tu veèkrat.
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
            Vector3 position = new (gridBlock.transform.position.x, 0.25f, gridBlock.transform.position.z);
            GameObject pathMarker = Instantiate(pathMarkerPrefab, position, Quaternion.identity);
            pathMarker.transform.parent = transform;
            pathMarkers.Add(pathMarker);
        }
    }

    public void SpawnMarker(Vector3 position)
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
        GameObject pathMarker = Instantiate(pathMarkerPrefab, location, Quaternion.identity);
        pathMarker.transform.parent = transform;
        pathMarkers.Add(pathMarker);
        
    }

    public void RemoveMarkers()
    {
        if (pathMarkers == null) return;
        foreach (GameObject pathMarker in pathMarkers)
        {
            Destroy(pathMarker);
        }
    }
}