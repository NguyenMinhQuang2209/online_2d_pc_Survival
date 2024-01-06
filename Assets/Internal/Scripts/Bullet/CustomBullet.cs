using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    private int damage = 1;
    private float speed = 1f;
    private Transform target = null;
    bool startAttack = false;

    private void Update()
    {
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
            if (startAttack)
            {
                Destroy(gameObject);
            }
        }
    }
    public void CustomBulletInit(Transform target, int damage, float speed, float delayDestroyTime)
    {
        startAttack = true;
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        Destroy(gameObject, delayDestroyTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            if (target.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}
