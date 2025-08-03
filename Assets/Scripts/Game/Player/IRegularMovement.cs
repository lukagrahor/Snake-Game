using UnityEngine;

public interface IRegularMovement
{
    public void OnGridBlockStay(Collider other);

    public void Rotate();

    public void SetRotation(float turnRotation);
}
