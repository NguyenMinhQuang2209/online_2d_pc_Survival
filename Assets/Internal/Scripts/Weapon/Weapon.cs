using UnityEngine;
using Unity.Netcode;
public abstract class Weapon : NetworkBehaviour
{
    [SerializeField] private int damage = 1;
    protected int plusDamage = 1;
    public int GetDamage()
    {
        return damage + plusDamage;
    }
}
