using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] List<Power> powers;
    int itemLimit = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SpawnNewItems()
    {
        List<Power> currentPowers = new List<Power>(powers);
        for(int i = 0; i < itemLimit; i++)
        {
            int index = Random.Range(0, currentPowers.Count);
            Debug.Log("Šime index " + index);
            //Vector3 tablePosition = transform.position;
            Vector3 itemPosition = new (-0.307f + i * 0.307f, -0.006f, 0.157f);
            Power newItem = Instantiate(currentPowers[index], transform);
            newItem.transform.localPosition = itemPosition;
            newItem.transform.localRotation = Quaternion.identity;
            newItem.name = newItem.GetType().FullName;
            Debug.Log("Šime " + newItem.name);
            currentPowers.RemoveAt(index);
        }
    }
}
