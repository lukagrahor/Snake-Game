using UnityEngine;
using System.Collections.Generic;
public class PathSpawner : MonoBehaviour
{
    [SerializeField] GameObject pathMarkerPrefab;
    List<GameObject> pathMarkers;

    public void SpawnMarkers(List<Vector3> path)
    {
        if (pathMarkers != null) RemoveMarkers();

        pathMarkers = new List<GameObject>();

        if (pathMarkerPrefab == null)
        {
            Debug.LogError("Path Marker Prefab is not assigned in the Inspector!");
            return;
        }

        foreach (Vector3 position in path)
        {
            GameObject pathMarker = Instantiate(pathMarkerPrefab, new Vector3(position.x, 0.25f, position.z), Quaternion.identity);
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