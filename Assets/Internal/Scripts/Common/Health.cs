using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    protected int plusHealth = 0;
    [SerializeField] private int currentHealth = 0;

    public virtual void HealthInit()
    {
        currentHealth = GetMaxHealth();
    }
    public int GetMaxHealth()
    {
        return maxHealth + plusHealth;
    }
    public int GetPlusHealth()
    {
        return plusHealth;
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public virtual void TakeDamage(int damage)
    {
        if (GameController.instance != null && !GameController.instance.CanDie())
        {
            return;
        }

        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (ShowTxtController.instance != null)
        {
            ShowTxtController.instance.ShowUI(transform.position, damage.ToString());
        }
        if (currentHealth == 0)
        {
            DestroyObject();
        }
    }
    public virtual void DestroyObject()
    {
        Destroy(gameObject);
    }
    public virtual void RecoverHealth(int v)
    {
        currentHealth = Mathf.Min(currentHealth + v, GetMaxHealth());
    }
}
