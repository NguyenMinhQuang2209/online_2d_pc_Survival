using UnityEngine;

public class CirclelyWeapon : Weapon
{
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
        for (int i = 0; i < 360; i += (360 / GetBulletAmount()))
        {
            Vector3 shootPosition = shootPos ? shootPos.position : transform.position;
            Bullet tempBullet = Instantiate(bullet, shootPosition, Quaternion.Euler(0f, 0f, i));
            tempBullet.BulletInit(GetDamage(), tempBullet.transform.up * 2f, GetSpeed(), GetDelayDieTime());
        }
    }
}
