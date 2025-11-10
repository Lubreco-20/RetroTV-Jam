using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    private List<GameObject> itemPool = new List<GameObject>();

    [Range(0f, 1f)] public float probSpawn;

    private void Start()
    {
        SpawnItemPool();
    }

    private void SpawnItemPool()
    {
        foreach (var item in items)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject itm = Instantiate(item, new Vector3(0f, -10f, 0f), Quaternion.identity, transform);
                itm.name = item.name;
                itemPool.Add(itm);
                itm.SetActive(false);
            }
        }
    }

    private void RequestItem(string itemName, Vector3 pos)
    {
        pos = new Vector3(pos.x, 1f, pos.z);

        GameObject obj = itemPool.FirstOrDefault(e => e.name == itemName && !e.activeInHierarchy);

        if (obj != null)
        {
            obj.transform.position = pos;
            obj.SetActive(true);
        }
    }

    public void GetRandomItem(Vector3 pos)
    {
        float prob = Random.Range(0f, 1f);
        if (prob > probSpawn) return;

        int rnd = Random.Range(0, items.Length);
        string itemName = items[rnd].name;

        RequestItem(itemName, pos);
    }
}