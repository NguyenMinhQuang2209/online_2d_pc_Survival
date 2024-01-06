using UnityEngine;

public class PlayerHealth : Health
{
    private void Start()
    {
        HealthInit();
    }
    public override void HealthInit()
    {
        base.HealthInit();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    public override void DestroyObject()
    {
        //base.DestroyObject();
        gameObject.SetActive(false);
    }
}
