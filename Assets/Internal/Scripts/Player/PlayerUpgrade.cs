using UnityEngine;
using Unity.Netcode;

public class PlayerUpgrade : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> plusHealth = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusMana = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusSpeed = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusBulletDamage = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusBulletSpeed = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusBulletTimeBwtAttack = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> plusBulletDelayDieTimer = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> recoverHealth = new NetworkVariable<int>(0);

    private PlayerHealth playerHealth;
    public override void OnNetworkSpawn()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        if (IsOwner)
        {
            if (UpgradeController.instance != null)
            {
                UpgradeController.instance.SetPlusValue(
                    plusHealth.Value,
                    plusMana.Value,
                    plusSpeed.Value,
                    plusBulletDamage.Value,
                    plusBulletSpeed.Value,
                    plusBulletTimeBwtAttack.Value,
                    plusBulletDelayDieTimer.Value
                    );
            }
        }

        if (IsServer)
        {
            if (recoverHealth.Value > 0)
            {
                recoverHealth.Value -= 1;
                playerHealth.RecoverHealthServerRpc(15);
            }
        }
    }

    public void PlusItem(DropItemName itemName)
    {

        switch (itemName)
        {
            case DropItemName.Health:
                plusHealth.Value += 1;
                break;
            case DropItemName.Mana:
                plusMana.Value += 1;
                break;
            case DropItemName.Speed:
                plusSpeed.Value += 1;
                break;
            case DropItemName.Damage:
                plusBulletDamage.Value += 1;
                break;
            case DropItemName.TimeBwtAttack:
                plusBulletTimeBwtAttack.Value += 1;
                break;
            case DropItemName.BulletSpeed:
                plusBulletSpeed.Value += 1;
                break;
            case DropItemName.DelayDieTime:
                plusBulletDelayDieTimer.Value += 1;
                break;
            case DropItemName.RecoverHealth:
                recoverHealth.Value += 1;
                break;
        }
    }
    public int GetPlusHealth()
    {
        return plusHealth.Value;
    }
    public int GetPlusMana()
    {
        return plusMana.Value;
    }
    public int GetPlusSpeed()
    {
        return plusSpeed.Value;
    }
    public int GetPlusBulletDamage()
    {
        return plusBulletDamage.Value;
    }
    public int GetPlusBulletSpeed()
    {
        return plusBulletSpeed.Value;
    }
    public int GetPlusBulletTimeBwtAttack()
    {
        return plusBulletTimeBwtAttack.Value;
    }
    public int GetPlusBulletDelayDieTimer()
    {
        return plusBulletDelayDieTimer.Value;
    }
}
