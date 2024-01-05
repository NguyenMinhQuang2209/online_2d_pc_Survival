using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 0;
    private Rigidbody2D rb;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Health>(out var health))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public void BulletInit(int damage, Vector3 shootDir, float speed, float destroyTime)
    {
        this.damage = damage;

        if (TryGetComponent<Rigidbody2D>(out rb))
        {
            rb.AddForce(speed * shootDir, ForceMode2D.Force);
        }

        Destroy(gameObject, destroyTime);
    }
}
