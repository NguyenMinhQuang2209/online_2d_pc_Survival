using Unity.Netcode;
using UnityEngine;

public abstract class Health : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<int> maxHealth = new NetworkVariable<int>(100);
    protected NetworkVariable<int> plusHealth = new NetworkVariable<int>(0);
    [SerializeField] private NetworkVariable<int> currentHealth = new NetworkVariable<int>(0);

    public virtual void HealthInit()
    {
        currentHealth.Value = GetMaxHealth();
    }
    public int GetMaxHealth()
    {
        return maxHealth.Value + plusHealth.Value;
    }
    public int GetPlusHealth()
    {
        return plusHealth.Value;
    }
    public int GetCurrentHealth()
    {
        return currentHealth.Value;
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void TakeDamageServerRpc(int damage)
    {
        if (GameController.instance != null && !GameController.instance.CanDie())
        {
            return;
        }

        currentHealth.Value = Mathf.Max(0, currentHealth.Value - damage);

        if (ShowTxtController.instance != null)
        {
            ShowTxtController.instance.ShowUIServerRpc(new[] { transform.position.x, transform.position.y, transform.position.z }, damage.ToString());
        }
        if (currentHealth.Value == 0)
        {
            DestroyObjectServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public virtual void DestroyObjectServerRpc()
    {
        Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public virtual void RecoverHealthServerRpc(int v)
    {
        currentHealth.Value = Mathf.Min(currentHealth.Value + v, GetMaxHealth());
    }
}
