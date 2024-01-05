using UnityEngine;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    protected int plusDamage = 1;
    public int GetDamage()
    {
        return damage + plusDamage;
    }
}
