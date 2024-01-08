using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DropSystemController : NetworkBehaviour
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
    public override void OnNetworkSpawn()
    {
        enabled = IsServer;
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

    [ServerRpc(RequireOwnership = false)]
    public void SpawnDropItemServerRpc(DropItemName itemName, float[] pos)
    {
        DropItem item = GetWorldItem(itemName);
        if (item != null)
        {
            DropItem temp = Instantiate(item, new(pos[0], pos[1], pos[2]), Quaternion.identity);
            if (temp.TryGetComponent<NetworkObject>(out var networkObject))
            {
                networkObject.Spawn();
            }
        }
    }

}
[System.Serializable]
public class DropItemStore
{
    public DropItemName itemName;
    public DropItem worldItem;
}
