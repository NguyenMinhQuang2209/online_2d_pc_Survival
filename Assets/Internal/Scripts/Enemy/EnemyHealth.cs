using Unity.Netcode;
using UnityEngine;

public class EnemyHealth : Health
{
    private Enemy enemy;
    [SerializeField] private float delayDestroyTimer = 2f;
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        HealthInit();
    }

    [ServerRpc(RequireOwnership = false)]
    public override void TakeDamageServerRpc(int damage)
    {
        base.TakeDamageServerRpc(damage);
        if (enemy != null)
        {
            enemy.EnemyGetHit();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public override void DestroyObjectServerRpc()
    {
        if (TryGetComponent<Collider2D>(out var collider2d))
        {
            collider2d.enabled = false;
        }
        if (enemy != null)
        {
            enemy.EnemyDie();
        }
        if (IsServer)
        {
            Destroy(gameObject, delayDestroyTimer);
        }
    }
}
