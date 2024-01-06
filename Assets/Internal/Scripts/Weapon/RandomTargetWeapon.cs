using UnityEngine;

public class RandomTargetWeapon : Weapon
{
    [SerializeField] private CustomBullet customBullet;
    [SerializeField] private float attackRadious = 1f;
    float currentTimeBwtAttack = 0f;
    private void Update()
    {
        currentTimeBwtAttack += Time.deltaTime;
        if (currentTimeBwtAttack >= GetTimeBwtAttack())
        {
            currentTimeBwtAttack = 0f;
            Shoot();
        }
    }
    public override void Shoot()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadious);
        if (hits.Length > 0)
        {
            bool isPlayer = false;
            Transform nextTarget = hits[0].transform;
            float currentDistance = Vector2.Distance(transform.position, nextTarget.position);
            for (int i = 0; i < hits.Length; i++)
            {
                Transform temp = hits[i].transform;
                if (temp.GetComponent<Health>() != null)
                {
                    if (temp.CompareTag(CommonController.PLAYER_TAG))
                    {
                        if (isPlayer)
                        {
                            float nextDistance = Vector2.Distance(transform.position, temp.position);
                            if (nextDistance < currentDistance)
                            {
                                nextTarget = temp;
                                currentDistance = nextDistance;
                            }
                        }
                        else
                        {
                            nextTarget = temp;
                            currentDistance = Vector2.Distance(transform.position, nextTarget.position);
                            isPlayer = true;
                        }
                    }
                    else
                    {
                        if (!isPlayer)
                        {
                            float nextDistance = Vector2.Distance(transform.position, temp.position);
                            if (nextDistance < currentDistance)
                            {
                                nextTarget = temp;
                                currentDistance = nextDistance;
                            }
                        }
                    }
                }
            }

            if (nextTarget != null)
            {
                CustomBullet customBull = Instantiate(customBullet, transform.position, Quaternion.identity);
                customBull.CustomBulletInit(nextTarget, GetDamage(), GetSpeed(), GetDelayDieTime());
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }
}
