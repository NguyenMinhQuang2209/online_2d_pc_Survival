using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public static UpgradeController instance;

    private int plusHealth = 0;
    private int plusMana = 0;
    private int plusSpeed = 0;
    private int plusBulletDamage = 0;
    private int plusBulletSpeed = 0;
    private int plusBulletTimeBwtAttack = 0;
    private int plusBulletDelayDieTimer = 0;

    [SerializeField] private List<UpgradeItem> upgradeItems = new();

    [SerializeField] private Transform upgradeUIContainer;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        foreach (Transform child in upgradeUIContainer)
        {
            Destroy(child.gameObject);
        }
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
    public void SetPlusValue(
        int plusHealth,
        int plusMana,
        int plusSpeed,
        int plusBulletDamage,
        int plusBulletSpeed,
        int plusBulletTimeBwtAttack,
        int plusBulletDelayDieTimer)
    {
        bool needUpgradeUI = false;
        if (this.plusHealth != plusHealth)
        {
            this.plusHealth = plusHealth;
            needUpgradeUI = true;
        }

        if (this.plusMana != plusMana)
        {
            this.plusMana = plusMana;
            needUpgradeUI = true;
        }
        if (this.plusSpeed != plusSpeed)
        {
            this.plusSpeed = plusSpeed;
            needUpgradeUI = true;
        }
        if (this.plusBulletSpeed != plusBulletSpeed)
        {
            this.plusBulletSpeed = plusBulletSpeed;
            needUpgradeUI = true;
        }
        if (this.plusBulletDamage != plusBulletDamage)
        {
            this.plusBulletDamage = plusBulletDamage;
            needUpgradeUI = true;
        }
        if (this.plusBulletDelayDieTimer != plusBulletDelayDieTimer)
        {
            this.plusBulletDelayDieTimer = plusBulletDelayDieTimer;
            needUpgradeUI = true;
        }
        if (this.plusBulletTimeBwtAttack != plusBulletTimeBwtAttack)
        {
            this.plusBulletTimeBwtAttack = plusBulletTimeBwtAttack;
            needUpgradeUI = true;
        }

        if (needUpgradeUI)
        {
            SetUpgradeUI();
        }
    }

    private void SetUpgradeUI()
    {
        foreach (Transform child in upgradeUIContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < upgradeItems.Count; i++)
        {
            UpgradeItem temp = upgradeItems[i];
            int v = 0;
            switch (temp.dropItemName)
            {
                case DropItemName.Health:
                    v = plusHealth;
                    break;
                case DropItemName.Mana:
                    v = plusMana;
                    break;
                case DropItemName.Speed:
                    v = plusSpeed;
                    break;
                case DropItemName.Damage:
                    v = plusBulletDamage;
                    break;
                case DropItemName.TimeBwtAttack:
                    v = plusBulletTimeBwtAttack;
                    break;
                case DropItemName.DelayDieTime:
                    v = plusBulletDelayDieTimer;
                    break;
                case DropItemName.BulletSpeed:
                    v = plusBulletSpeed;
                    break;
            }
            if (v > 0)
            {
                UpgradeUIItem tempUI = Instantiate(temp.uiItem, upgradeUIContainer);
                tempUI.UpgradeUItemInit(v.ToString());
            }
        }
    }
}
[Serializable]
public class UpgradeItem
{
    public UpgradeUIItem uiItem;
    public DropItemName dropItemName;
}
