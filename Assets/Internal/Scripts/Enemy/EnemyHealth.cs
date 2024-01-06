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
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        if (enemy != null)
        {
            enemy.EnemyGetHit();
        }
    }
    public override void DestroyObject()
    {
        if (TryGetComponent<Collider2D>(out var collider2d))
        {
            collider2d.enabled = false;
        }
        if (enemy != null)
        {
            enemy.EnemyDie();
        }
        Destroy(gameObject, delayDestroyTimer);
    }
}
