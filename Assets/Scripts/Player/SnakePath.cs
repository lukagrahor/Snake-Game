using System.Collections.Generic;
using UnityEngine;

public class SnakePath : MonoBehaviour
{
    [SerializeField] PathSpawner pathSpawner;
    List<SnakePathMarker> Path { get; set; }

    private void Start()
    {
        Path = new List<SnakePathMarker>();
    }

    public void AddToPath(Vector3 location, float nextRotation)
    {
        Path.Add(new SnakePathMarker(location, nextRotation));
        pathSpawner.SpawnMarker(location);
    }
}