using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnBulletController : NetworkBehaviour
{
    [SerializeField] private List<Bullet> bullets = new();
    [SerializeField] private List<CustomBullet> customBullets = new();

    public static SpawnBulletController instance;

    public override void OnNetworkSpawn()
    {
        enabled = IsServer;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletItemServerRpc(int pos, int damage, float speed, float detroyTime, float[] position, float[] rotate, ulong target)
    {
        CustomBullet customBull = Instantiate(customBullets[pos], new(position[0], position[1], position[2]), Quaternion.Euler(new(rotate[0], rotate[1], rotate[2])));
        if (customBull.TryGetComponent<NetworkObject>(out var networkItem))
        {
            networkItem.Spawn();
        }
        customBull.CustomBulletInitServerRpc(target, damage, speed, detroyTime);
    }
    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletItemServerRpc(int pos, int damage, float speed, float detroyTime, float[] position, float[] rotate)
    {
        Bullet tempBullet = Instantiate(bullets[pos], new(position[0], position[1], position[2]), Quaternion.Euler(new Vector3(rotate[0], rotate[1], rotate[2])));
        if (tempBullet.TryGetComponent<NetworkObject>(out var networkObject))
        {
            networkObject.Spawn();
        }
        tempBullet.BulletInitServerRpc(damage, tempBullet.transform.up * 2f, speed, detroyTime);
    }
}
