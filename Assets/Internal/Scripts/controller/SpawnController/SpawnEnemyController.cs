using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnEnemyController : NetworkBehaviour
{
    public Vector2 randomXAxisTop = Vector2.one;
    public Vector2 randomXAxisDown = Vector2.one;
    public Vector2 randomYAxisTop = Vector2.one;
    public Vector2 randomYAxisDown = Vector2.one;

    public Vector2 allXAxis = Vector2.one;
    public Vector2 allYAxis = Vector2.one;

    public List<EnemySpawnConfig> enemySpawnConfig = new();

    private List<EnemySpawnConfigIndex> currentEnemySpawn = new();

    float currentSpawnTimer = 0f;
    int currentDay = -1;

    public override void OnNetworkSpawn()
    {
        enabled = IsServer;
    }
    private void Update()
    {
        if (IsServer)
        {
            int currentDate = DayNightController.instance.currentDay.Value;
            if (currentDay != currentDate)
            {
                currentDay = currentDate;
                currentSpawnTimer = 0f;
                currentEnemySpawn?.Clear();
                for (int i = 0; i < enemySpawnConfig.Count; i++)
                {
                    EnemySpawnConfig tempItem = enemySpawnConfig[i];
                    if (tempItem.day <= currentDay)
                    {
                        currentEnemySpawn.Add(new(tempItem.enemy, 0, tempItem.spawnTime));
                    }
                }
            }
            else
            {
                currentSpawnTimer += Time.deltaTime;
                for (int i = 0; i < currentEnemySpawn.Count; i++)
                {
                    EnemySpawnConfigIndex tempItem = currentEnemySpawn[i];
                    if (currentSpawnTimer >= (tempItem.spawnTime * tempItem.index))
                    {
                        tempItem.index += 1;
                        SpawnEnemy(tempItem.enemy);
                    }
                }
            }
        }
    }

    public void SpawnEnemy(Enemy enemy)
    {
        int randomPos = Random.Range(0, 4);

        Vector2 xAxis = Vector2.zero;
        Vector2 yAxis = Vector2.zero;

        switch (randomPos)
        {
            case 0:
                xAxis = randomXAxisTop;
                yAxis = allYAxis;
                break;
            case 1:
                xAxis = randomXAxisDown;
                yAxis = allYAxis;
                break;
            case 2:
                xAxis = allXAxis;
                yAxis = randomYAxisTop;
                break;
            case 3:
                xAxis = allXAxis;
                yAxis = randomYAxisDown;
                break;
        }

        float ranX = Random.Range(Mathf.Min(xAxis.x, xAxis.y), Mathf.Max(xAxis.x, xAxis.y));
        float ranY = Random.Range(Mathf.Min(yAxis.x, yAxis.y), Mathf.Max(yAxis.x, yAxis.y));

        Enemy tempEnemy = Instantiate(enemy, new Vector3(ranX, ranY, 0f), Quaternion.identity);
        if (tempEnemy.TryGetComponent<NetworkObject>(out var networkObject))
        {
            networkObject.Spawn();
        }
    }
}
[System.Serializable]
public class EnemySpawnConfig
{
    public Enemy enemy;
    public int day = 0;
    public float spawnTime = 1f;
    public EnemySpawnConfig(Enemy enemy, int day, float spawnTime)
    {
        this.enemy = enemy;
        this.day = day;
        this.spawnTime = spawnTime;
    }
}
public class EnemySpawnConfigIndex
{
    public Enemy enemy;
    public int index = 0;
    public float spawnTime = 1f;
    public EnemySpawnConfigIndex(Enemy enemy, int index, float spawnTime)
    {
        this.enemy = enemy;
        this.index = index;
        this.spawnTime = spawnTime;
    }
}