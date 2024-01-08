using Unity.Netcode;
using UnityEngine;

public class CustomBullet : NetworkBehaviour
{
    private int damage = 1;
    private float speed = 1f;
    private Transform target = null;

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
    }

    [ServerRpc]
    public void CustomBulletInitServerRpc(int targetId, int damage, float speed, float delayDestroyTime)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetInstanceID() == targetId)
            {
                target = players[i].transform;
                break;
            }
        }
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            this.damage = damage;
            this.speed = speed;
            Destroy(gameObject, delayDestroyTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != null)
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
}
