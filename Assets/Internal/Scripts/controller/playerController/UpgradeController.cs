using System;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController instance;

    public event EventHandler<DropItemName> UpgrdateEvent;

    private int plusHealth = 0;
    private int plusMana = 0;
    private int plusSpeed = 0;
    private int plusBulletDamage = 0;
    private int plusBulletSpeed = 0;
    private int plusBulletTimeBwtAttack = 0;
    private int plusBulletDelayDieTimer = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public int GetPlusHealth()
    {
        return plusHealth;
    }
    public int GetPlusMana()
    {
        return plusMana;
    }
    public int GetPlusSpeed()
    {
        return plusSpeed;
    }
    public int GetPlusBulletDamage()
    {
        return plusBulletDamage;
    }
    public int GetPlusBulletSpeed()
    {
        return plusBulletSpeed;
    }
    public int GetPlusBulletTimeBwtAttack()
    {
        return plusBulletTimeBwtAttack;
    }
    public int GetPlusBulletDelayDieTimer()
    {
        return plusBulletDelayDieTimer;
    }
    public void PlusItem(DropItemName itemName)
    {
        switch (itemName)
        {
            case DropItemName.Health:
                plusHealth += 1;
                break;
            case DropItemName.Mana:
                plusMana += 1;
                break;
            case DropItemName.Speed:
                plusSpeed += 1;
                break;
            case DropItemName.Damage:
                plusBulletDamage += 1;
                break;
            case DropItemName.TimeBwtAttack:
                plusBulletTimeBwtAttack += 1;
                break;
            case DropItemName.BulletSpeed:
                plusBulletSpeed += 1;
                break;
            case DropItemName.DelayDieTime:
                plusBulletDelayDieTimer += 1;
                break;
        }
        UpgrdateEvent?.Invoke(this, itemName);
    }
}
