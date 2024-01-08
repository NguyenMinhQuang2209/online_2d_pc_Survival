using Unity.Netcode;
using UnityEngine;

public class DropItem : NetworkBehaviour
{
    [SerializeField] private DropItemName dropItemName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            UpgradeController.instance.PlusItem(dropItemName);
            PlayerPickupServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerPickupServerRpc()
    {
        Destroy(gameObject);
    }
}
