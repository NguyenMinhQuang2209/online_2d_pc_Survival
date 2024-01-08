using System.Collections.Generic;
using UnityEngine;

public class DropItemObject : MonoBehaviour
{
    [Tooltip("Using %")]
    [SerializeField] private float dropRate = 5f;
    [SerializeField] private List<DropItemName> dropItemName;
    public void DropItem()
    {
        float randomRate = Random.Range(0f, 100f);
        if (randomRate <= dropRate)
        {
            int randomItem = Random.Range(0, dropItemName.Count);
            DropSystemController.instance.SpawnDropItemServerRpc(dropItemName[randomItem], new[] { transform.position.x, transform.position.y, transform.position.z });
        }
    }
}
