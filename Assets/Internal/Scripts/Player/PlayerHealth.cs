using UnityEngine;

public class PlayerHealth : Health
{
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
    }
}
