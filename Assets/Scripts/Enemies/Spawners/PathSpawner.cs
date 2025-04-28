using UnityEngine;
using System.Collections.Generic;
public class PathSpawner : MonoBehaviour
{
    [SerializeField] GameObject pathMarkerPrefab;
    List<GameObject> pathMarkers;

    public void SpawnMarkers(List<GridObject> path)
    {
        if (pathMarkers != null) RemoveMarkers();

        pathMarkers = new List<GameObject>();

        if (pathMarkerPrefab == null)
        {
            Debug.LogError("Path Marker Prefab is not assigned in the Inspector!");
            return;
        }

        foreach (GridObject gridBlock in path)
        {
            Vector3 position = new Vector3(gridBlock.transform.position.x, 0.25f, gridBlock.transform.position.z);
            GameObject pathMarker = Instantiate(pathMarkerPrefab, position, Quaternion.identity);
            pathMarker.transform.parent = transform;
            pathMarkers.Add(pathMarker);
        }
    }

    public void RemoveMarkers()
    {
        foreach (GameObject pathMarker in pathMarkers)
        {
            Destroy(pathMarker);
        }
    }
}