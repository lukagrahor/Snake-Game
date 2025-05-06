using UnityEngine;

public class SnakePathMarker
{
    Vector3 location;
    float nextRotation;

    public SnakePathMarker(Vector3 location, float nextRotation)
    {
        this.location = location;
        this.nextRotation = nextRotation;
    }
}