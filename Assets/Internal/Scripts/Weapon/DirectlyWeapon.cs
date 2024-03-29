using UnityEngine;

public class DirectlyWeapon : Weapon
{
    /*float currentTimeBwtAttack = 0f;
    private void Update()
    {
        if (GameController.instance != null && !GameController.instance.CanShoot())
        {
            return;
        }

        currentTimeBwtAttack += Time.deltaTime;
        if (currentTimeBwtAttack >= GetTimeBwtAttack())
        {
            currentTimeBwtAttack = 0f;
            Shoot();
        }
    }*/
    public override void Shoot()
    {
        Transform parent = transform.parent;
        if (parent != null)
        {
            for (int i = 0; i < GetBulletAmount(); i++)
            {
                float angle = i == 0 ? 0 : (i % 2) == 0 ? i * shootAngle : (i - 1) * -shootAngle;
                Vector3 shootPosition = shootPos ? shootPos.position : transform.position;

                /*Bullet tempBullet = Instantiate(bullet, shootPosition, Quaternion.Euler(0f, 0f, parent.eulerAngles.z + angle + 270f));
                if (tempBullet.TryGetComponent<NetworkObject>(out var networkObject))
                {
                    networkObject.Spawn();
                }
                tempBullet.BulletInitServerRpc(GetDamage(), tempBullet.transform.up * 2f, GetSpeed(), GetDelayDieTime());*/
                SpawnBulletController.instance.SpawnBulletItemServerRpc(
                    bulletPosition,
                    GetDamage(),
                    GetSpeed(),
                    GetDelayDieTime(),
                    new float[] { shootPosition.x, shootPosition.y, shootPosition.z },
                    new float[] { 0f, 0f, parent.eulerAngles.z + angle + 270f });
            }
        }
    }
}
