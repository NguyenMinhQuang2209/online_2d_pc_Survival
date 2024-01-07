using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropSystemController : MonoBehaviour
{
    public static DropSystemController instance;

    [SerializeField] private List<DropItemStore> dropItems = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public DropItem GetWorldItem(DropItemName itemName)
    {
        foreach (DropItemStore item in dropItems)
        {
            if (item.itemName == itemName)
            {
                return item.worldItem;
            }
        }
        return null;
    }
    public void SpawnDropItem(DropItemName itemName, Vector3 pos)
    {
        DropItem item = GetWorldItem(itemName);
        if (item != null)
        {
            Instantiate(item, pos, Quaternion.identity);
        }
    }

}
[System.Serializable]
public class DropItemStore
{
    public DropItemName itemName;
    public DropItem worldItem;
}
