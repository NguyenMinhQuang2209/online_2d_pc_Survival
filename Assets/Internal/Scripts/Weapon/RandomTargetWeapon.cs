using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomTargetWeapon : Weapon
{
    [SerializeField] private float attackRadious = 1f;
    //float currentTimeBwtAttack = 0f;

    Transform rootParent = null;
    private void Start()
    {
        rootParent = transform.parent;
    }
    /*private void Update()
    {
        if (GameController.instance != null && !GameController.instance.CanShoot())
        {
            return;
        }

        currentTimeBwtAttack += Time.deltaTime;
        if (currentTimeBwtAttack >= GetTimeBwtAttack())
        {
            Shoot();
        }
    }*/
    public override void Shoot()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadious);
        List<Transform> tempHits = new();
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Transform tempHit = hits[i].transform;

                if (tempHit != rootParent)
                {
                    tempHits.Add(tempHit);
                }

            }

            if (tempHits == null || tempHits.Count == 0)
            {
                return;
            }

            bool isPlayer = false;
            Transform nextTarget = tempHits[0].transform;
            float currentDistance = Vector2.Distance(transform.position, nextTarget.position);
            for (int i = 0; i < tempHits.Count; i++)
            {
                Transform temp = tempHits[i];
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

            if (nextTarget != null && nextTarget.TryGetComponent<NetworkObject>(out var networkObject))
            {
                //currentTimeBwtAttack = 0f;
                /*CustomBullet customBull = Instantiate(customBullet, transform.position, Quaternion.identity);
                if (customBull.TryGetComponent<NetworkObject>(out var networkItem))
                {
                    networkItem.Spawn();
                }
                customBull.CustomBulletInitServerRpc(nextTarget.gameObject.GetInstanceID(), GetDamage(), GetSpeed(), GetDelayDieTime());*/


                SpawnBulletController.instance.SpawnBulletItemServerRpc(
                    bulletPosition,
                    GetDamage(),
                    GetSpeed(),
                    GetDelayDieTime(),
                    new float[] { transform.position.x, transform.position.y, transform.position.z },
                    new float[] { 0f, 0f, 0f },
                    networkObject.NetworkObjectId);
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }
}
