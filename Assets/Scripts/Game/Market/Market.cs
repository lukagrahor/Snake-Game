using System.Collections.Generic;
using UnityEngine;

public class Market : MonoBehaviour
{
    [SerializeField] Snake snake;
    [SerializeField] List<Power> powers;
    [SerializeField] MarketSign marketSign;
    public Power SelectedPower;
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
        powersToSpawn = RemoveBoughtItems(powersToSpawn);
        spawnedPowers = new List<Power>();
        for(int i = 0; i < itemLimit; i++)
        {
            if (powersToSpawn.Count == 0) return;
            int index = Random.Range(0, powersToSpawn.Count);
            Debug.Log("Šime index " + index);
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

    List<Power> RemoveBoughtItems(List<Power> spawnedPowers)
    {
        int i = 0;
        while (i < spawnedPowers.Count)
        {
            if (spawnedPowers.Count == 0) break;
            Power power = spawnedPowers[i];
            if (power.bought)
            {
                spawnedPowers.Remove(power);
                Destroy(power.gameObject);
                continue;
            }
            i++;
        }
        return spawnedPowers;
    }

    public void DespawnItems()
    {
        int i = spawnedPowers.Count - 1;
        while (i >= 0)
        {
            Power power = spawnedPowers[i];
            spawnedPowers.Remove(power);
            Destroy(power.gameObject);
            i--;
        }
    }

    public bool CheckForFunds(int price)
    {
        if (snake.NewLevelSize - 2 >= price) return true;
        return false;
    }

    public void UnselectPreviouslySelected(int partCount, Power keepSelected)
    {
        SelectedPower = keepSelected;
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

    public void ConfirmPurchase()
    {
        Debug.Log($"Bought power: {SelectedPower.name}");
        marketSign.RemoveParts(SelectedPower.Price);
        spawnedPowers.Remove(SelectedPower);
        // hodi skozi vse moèi in preveri ali je tip trenutno izbrane moèi enak tipu moèi v listu
        Debug.Log($"Selected power: {SelectedPower}, type: {SelectedPower.GetType()}");
        foreach (Power power in powers)
        {
            Debug.Log($"Power: {power}, type: {power.GetType()}");
            if (SelectedPower.GetType() == power.GetType())
            {
                Debug.Log($"Ta je tista!!! Power: {power}, type: {power.GetType()}");
                power.bought = true;
                break;
            }
        }
        Destroy(SelectedPower.gameObject);
    }
}
