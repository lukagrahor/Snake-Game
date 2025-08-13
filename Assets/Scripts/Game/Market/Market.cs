using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] Snake snake;
    [SerializeField] List<Power> powers;
    [SerializeField] MarketSign marketSign;
    List<Power> powersToSpawn;
    List<Power> spawnedPowers;
    int itemLimit = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnDisable()
    {
        foreach (Power power in spawnedPowers)
        {
            Destroy(power.gameObject);
        }
    }
    public void SpawnNewItems()
    {
        powersToSpawn = new List<Power>(powers);
        spawnedPowers = new List<Power>();
        for(int i = 0; i < itemLimit; i++)
        {
            int index = Random.Range(0, powersToSpawn.Count);
            Debug.Log("Šime index " + index);
            //Vector3 tablePosition = transform.position;
            Vector3 itemPosition = new (-0.307f + i * 0.307f, -0.006f, 0.157f);
            Power newItem = Instantiate(powersToSpawn[index], transform);
            newItem.transform.localPosition = itemPosition;
            newItem.transform.localRotation = Quaternion.identity;
            newItem.name = newItem.GetType().FullName;
            Debug.Log("Šime " + newItem.name);
            powersToSpawn.RemoveAt(index);
            spawnedPowers.Add(newItem);
        }
    }

    public bool CheckForFunds(int price)
    {
        if (snake.NewLevelSize - 2 >= price) return true;
        return false;
    }

    public void UnselectPreviouslySelected(int partCount, Power keepSelected)
    {
        foreach (Power power in spawnedPowers)
        {
            if (keepSelected != power) power.UnselectItem();
        }
        RemoveMarks(partCount);
    }

    public void RemoveMarks(int partCount)
    {
        marketSign.RemoveMarks(partCount);
    }

    public void MarkParts(int partCount)
    {
        marketSign.MarkParts(partCount);
    }
}
