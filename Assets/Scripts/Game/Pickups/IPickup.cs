using UnityEngine;

public interface IPickup
{
    public void Use();
    public void SetSpawner(ObjectSpawner spawner);

    public void SetNewPosition(Vector3 position);
}
