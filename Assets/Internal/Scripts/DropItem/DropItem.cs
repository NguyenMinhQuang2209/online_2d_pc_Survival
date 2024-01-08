using Unity.Netcode;
using UnityEngine;

public class DropItem : NetworkBehaviour
{
    [SerializeField] private DropItemName dropItemName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMovement>(out var playerMovement))
        {

            PlayerPickupServerRpc(playerMovement.NetworkObjectId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PlayerPickupServerRpc(ulong id)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            var p = players[i];
            if (p.TryGetComponent<PlayerUpgrade>(out var playerUpgrade))
            {
                if (playerUpgrade.NetworkObjectId == id)
                {
                    playerUpgrade.PlusItem(dropItemName);
                    break;
                }
            }
        }
        Destroy(gameObject);
    }

}
