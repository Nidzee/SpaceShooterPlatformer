using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    [SerializeField] List<LootItem> _lootItemsCollection;

    public void DropLoot()
    {
        // Skip if list is missing
        if (_lootItemsCollection == null)
        {
            return;
        }

        // Skip if no items in bag
        if (_lootItemsCollection.Count <= 0)
        {
            return;
        }


        // Create list of items that will drop
        List<BasicDropItem> itemsToDrop = new List<BasicDropItem>();

        // Get random number fo probability
        int randomNumber = UnityEngine.Random.Range(0, 101);

        // Check if probability is OK for each item
        foreach (var item in _lootItemsCollection)
        {
            // Skip item if not valid
            if (!item.IsItemValid())
            {
                continue;
            }

            if (randomNumber <= item.probability)
            {
                itemsToDrop.Add(item.lootItem);
            }
        }

        SpawnLoot(itemsToDrop);
    }

    void SpawnLoot(List<BasicDropItem> lootToSpawn)
    {
        foreach (var item in lootToSpawn)
        {
            GameObject lootObject = Instantiate(item.gameObject, transform.position, Quaternion.identity);
            Vector3 randomDirection = new Vector3(Random.Range(-1, 1), 1f, Random.Range(-1, 1));
            lootObject.GetComponent<Rigidbody>().AddForce(randomDirection.normalized * 3, ForceMode.Impulse);
        }
    }
}