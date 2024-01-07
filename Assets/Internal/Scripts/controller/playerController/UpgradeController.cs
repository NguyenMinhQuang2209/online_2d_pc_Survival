using System;
using System.Collections.Generic;
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
        SetUpgradeUI(itemName);
        UpgrdateEvent?.Invoke(this, itemName);
    }
    private void SetUpgradeUI(DropItemName itemName)
    {
        int tempV = 0;
        switch (itemName)
        {
            case DropItemName.Health:
                tempV = plusHealth;
                break;
            case DropItemName.Mana:
                tempV = plusMana;
                break;
            case DropItemName.Speed:
                tempV = plusSpeed;
                break;
            case DropItemName.Damage:
                tempV = plusBulletDamage;
                break;
            case DropItemName.TimeBwtAttack:
                tempV = plusBulletTimeBwtAttack;
                break;
            case DropItemName.BulletSpeed:
                tempV = plusBulletSpeed;
                break;
            case DropItemName.DelayDieTime:
                tempV = plusBulletDelayDieTimer;
                break;

        }
        for (int i = 0; i < upgradeUIContainer.childCount; i++)
        {
            Transform tempChild = upgradeUIContainer.GetChild(i);
            if (tempChild.TryGetComponent<UpgradeUIItem>(out var item))
            {
                if (item.dropItemName == itemName)
                {
                    item.UpgradeUItemInit(tempV.ToString());
                    return;
                }
            }
        }
        UpgradeUIItem tempUIItem = null;
        for (int i = 0; i < upgradeItems.Count; i++)
        {
            UpgradeItem temp = upgradeItems[i];
            if (temp.dropItemName == itemName)
            {
                tempUIItem = temp.uiItem;
                break;
            }
        }
        if (tempUIItem != null)
        {
            UpgradeUIItem temp = Instantiate(tempUIItem, upgradeUIContainer);
            temp.UpgradeUItemInit(tempV.ToString());
        }
    }
}
[Serializable]
public class UpgradeItem
{
    public UpgradeUIItem uiItem;
    public DropItemName dropItemName;
}
