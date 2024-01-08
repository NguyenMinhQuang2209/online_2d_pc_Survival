using Unity.Netcode;
using UnityEngine;

public class SpawnPlayerController : MonoBehaviour
{
    public float waitTime = 5f;
    public Vector2 randomXAxis = Vector2.one;
    public Vector2 randomYAxis = Vector2.one;
    private void Start()
    {
        //Invoke(nameof(HandleSpawnPlayer), waitTime);
    }
    private void HandleSpawnPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].TryGetComponent<PlayerMovement>(out var playerMovement))
            {
                if (playerMovement.IsOwner)
                {
                    float x = Random.Range(Mathf.Min(randomXAxis.x, randomXAxis.y), Mathf.Max(randomXAxis.x, randomXAxis.y));
                    float y = Random.Range(Mathf.Min(randomYAxis.x, randomYAxis.y), Mathf.Max(randomYAxis.x, randomYAxis.y));
                    players[i].transform.position = new Vector2(x, y);
                    break;
                }
            }
        }
    }
}
