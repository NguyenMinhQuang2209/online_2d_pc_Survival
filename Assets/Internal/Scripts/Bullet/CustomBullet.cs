using Unity.Netcode;
using UnityEngine;

public class CustomBullet : NetworkBehaviour
{
    private int damage = 0;
    private float speed = 1f;
    [SerializeField] private Transform target = null;

    private NetworkVariable<ulong> objectId = new NetworkVariable<ulong>();

    bool startBullet = false;

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            /*float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance <= 0.1f)
            {
                if (target.TryGetComponent<Health>(out var health))
                {
                    health.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }*/
        }
        else
        {
            if (startBullet)
            {
                Destroy(gameObject);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CustomBulletInitServerRpc(ulong targetId, int damage, float speed, float delayDestroyTime)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].TryGetComponent<NetworkObject>(out var networkObject))
            {
                if (networkObject.NetworkObjectId == targetId)
                {
                    target = enemies[i].transform;
                    break;
                }
            }
        }
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            objectId.Value = targetId;
            this.damage = damage;
            this.speed = speed;
            startBullet = true;
            Destroy(gameObject, delayDestroyTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<NetworkObject>(out var networkObject))
        {
            if (networkObject.NetworkObjectId == objectId.Value)
            {
                CheckColliderServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CheckColliderServerRpc()
    {
        if (target != null)
        {
            if (target.gameObject.TryGetComponent<Health>(out var health))
            {
                health.TakeDamageServerRpc(damage);
                Destroy(gameObject);
            }
        }
    }
}
