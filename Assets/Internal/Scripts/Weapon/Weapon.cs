using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] protected float speed = 1f;
    [SerializeField] protected float timeBwtAttack = 1f;
    [SerializeField] protected float delayDieTimer = 1f;
    [SerializeField] protected int bulletAmount = 1;
    [SerializeField] protected Bullet bullet;
    [SerializeField] protected float shootAngle = 8f;
    [SerializeField] protected Transform shootPos;

    [Space(5)]
    [Header("Plus config")]
    [SerializeField] private int plusBulletDamage = 0;
    [SerializeField] private float plusBulletSpeed = 0f;
    [SerializeField] private float plusBulletTimeBwtAttack = 0f;
    [SerializeField] private float plusBulletDelayDieTimer = 0f;
    [SerializeField] private int plusBulletAmount = 0;
    public int GetDamage()
    {
        int plusV = UpgradeController.instance != null ? UpgradeController.instance.GetPlusBulletDamage() : 0;
        return damage + (plusBulletDamage * plusV);
    }
    public float GetSpeed()
    {
        int plusV = UpgradeController.instance != null ? UpgradeController.instance.GetPlusBulletSpeed() : 0;
        return speed + (plusBulletSpeed * plusV);
    }
    public float GetTimeBwtAttack()
    {
        int plusV = UpgradeController.instance != null ? UpgradeController.instance.GetPlusBulletTimeBwtAttack() : 0;
        return timeBwtAttack - (plusBulletTimeBwtAttack * plusV);
    }
    public float GetDelayDieTime()
    {
        int plusV = UpgradeController.instance != null ? UpgradeController.instance.GetPlusBulletDelayDieTimer() : 0;
        return delayDieTimer + (plusBulletDelayDieTimer * plusV);
    }
    public int GetBulletAmount()
    {
        return bulletAmount + plusBulletAmount;
    }
    public virtual void Shoot()
    {

    }
}
