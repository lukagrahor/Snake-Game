using UnityEngine;

public class SpitParticle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float moveSpeed = 4f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);
    }
}
