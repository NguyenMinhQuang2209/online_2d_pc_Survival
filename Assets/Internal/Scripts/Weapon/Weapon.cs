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
    protected int plusDamage = 1;
    protected int plusBulletAmount = 1;
    protected float plusSpeed = 1f;
    protected float reduceTimeBwtAttack = 0f;
    protected float plusDelayDieTime = 1f;
    public int GetDamage()
    {
        return damage + plusDamage;
    }
    public float GetSpeed()
    {
        return speed + plusSpeed;
    }
    public float GetTimeBwtAttack()
    {
        return timeBwtAttack - reduceTimeBwtAttack;
    }
    public float GetDelayDieTime()
    {
        return delayDieTimer + plusDelayDieTime;
    }
    public int GetBulletAmount()
    {
        return plusBulletAmount + bulletAmount;
    }
    public virtual void Shoot()
    {

    }
}
