using Unity.Netcode;
using UnityEngine;

public class ChooseBox : NetworkBehaviour
{
    [SerializeField] private Weapon weapon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement movement))
        {
            movement.EquipmentWeapon(weapon);
        }
    }
}
