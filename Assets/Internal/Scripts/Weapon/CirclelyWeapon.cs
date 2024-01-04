using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclelyWeapon : Weapon
{
    private void Update()
    {
        if (!IsOwner) return;
        MouseRotation();
    }
    private void MouseRotation()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDir = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new(0f, 0f, angle));
    }
}
