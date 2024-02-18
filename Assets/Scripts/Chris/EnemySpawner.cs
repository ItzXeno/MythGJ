using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Fields and Properties
    private Camera cam;
    private float camHeight;
    private float camWidth;

    public GameObject verticalEnemyPrefab;
    public GameObject horizontalEnemyPrefab;
    public GameObject zigzagEnemyPrefab;

    private int waveCount = 0;
    [SerializeField]
    private float timeBetweenSpawns = 0.5f;
    [SerializeField]// Time between each enemy spawn
    private int enemiesPerWave = 0;
    #endregion

    #region Enums
    enum SpawnSide { Top, Left, Right }
    #endregion

    #region Unity Methods
    void Start()
    {
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        DrawSpawnLines();
    }
    #endregion

    #region Spawn Logic
    IEnumerator SpawnWaves()
    {
        while (true) // Infinite loop to keep spawning waves
        {
            // Update logic to alternate enemy spawn types based on the wave count
            for (int i = 0; i < enemiesPerWave; i++)
            {
                // After the first two waves, start including horizontal enemies
                if (waveCount > 2)
                {
                    SpawnRandomEnemyType();
                }
                else
                {
                    // Initially, only spawn vertical enemies from the top
                    SpawnEnemy(SpawnSide.Top, verticalEnemyPrefab);
                }
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
            waveCount++;
           // Consider if you still need waveNumber or can merge with waveCount
            yield return new WaitForSeconds(5f); // Wait time before next wave
        }
    }
    void SpawnRandomEnemyType()
    {
        if (waveCount <= 2)
        {
            // Spawn only vertical enemies from the top
            SpawnEnemy(SpawnSide.Top, verticalEnemyPrefab);
        }
        else if (waveCount <= 7) // Adjusted to 7 to account for 2 initial + 5 horizontal waves
        {
            // Randomly choose to spawn vertical or horizontal enemies
            if (Random.Range(0, 2) == 0) // 50% chance
            {
                SpawnEnemy(SpawnSide.Top, verticalEnemyPrefab);
            }
            else
            {
                // Choose side for horizontal enemies
                SpawnSide side = Random.Range(0, 2) == 0 ? SpawnSide.Left : SpawnSide.Right;
                SpawnEnemy(side, horizontalEnemyPrefab);
            }
        }
        else
        {
            // Now include zigzag enemies in the random choice
            int enemyType = Random.Range(0, 3); // Now 0 to 2 for three types
            SpawnSide side;
            GameObject enemyPrefab;

            switch (enemyType)
            {
                case 0: // Vertical
                    side = SpawnSide.Top;
                    enemyPrefab = verticalEnemyPrefab;
                    break;
                case 1: // Horizontal
                    side = Random.Range(0, 2) == 0 ? SpawnSide.Left : SpawnSide.Right;
                    enemyPrefab = horizontalEnemyPrefab;
                    break;
                default: // Zigzag
                    // Zigzag enemies can spawn from any side
                    side = (SpawnSide)Random.Range(0, 3); // Casting to SpawnSide
                    enemyPrefab = zigzagEnemyPrefab;
                    break;
            }

            SpawnEnemy(side, enemyPrefab);
        }
    }
    void SpawnEnemy(SpawnSide side, GameObject enemyPrefab)
    {
        Vector2 spawnPosition = GetSpawnPosition(side);
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    GameObject GetEnemyPrefabForWave()
    {
        if (waveCount <= 3)
        {
            return verticalEnemyPrefab;
        }
        else if (waveCount <= 6)
        {
            return horizontalEnemyPrefab;
        }
        else
        {
            return zigzagEnemyPrefab;
        }
    }

    SpawnSide GetSpawnSideForWave()
    {
        if (waveCount <= 2)
        {
            return SpawnSide.Top;
        }
        else if (waveCount % 2 == 0)
        {
            return SpawnSide.Left;
        }
        else
        {
            return SpawnSide.Right;
        }
    }

    Vector2 GetSpawnPosition(SpawnSide side)
    {
        float x, y;

        switch (side)
        {
            case SpawnSide.Top:
                x = Random.Range(-camWidth / 3, camWidth / 3);
                y = cam.orthographicSize;
                break;
            case SpawnSide.Left:
                x = -camWidth / 2;
                y = Random.Range(-camHeight / 3, camHeight / 3);
                break;
            case SpawnSide.Right:
                x = camWidth / 2;
                y = Random.Range(-camHeight / 3, camHeight / 3);
                break;
            default:
                x = 0;
                y = 0;
                break;
        }

        return cam.transform.position + new Vector3(x, y, 0);
    }
    #endregion

    #region Debug Methods
    void DrawSpawnLines()
    {
        Vector3 camPosition = cam.transform.position;
        Debug.DrawLine(new Vector3(camPosition.x - camWidth / 2, camPosition.y + camHeight / 2, 0), new Vector3(camPosition.x + camWidth / 2, camPosition.y + camHeight / 2, 0), Color.red);
        Debug.DrawLine(new Vector3(camPosition.x - camWidth / 2, camPosition.y - camHeight / 2, 0), new Vector3(camPosition.x - camWidth / 2, camPosition.y + camHeight / 2, 0), Color.green);
        Debug.DrawLine(new Vector3(camPosition.x + camWidth / 2, camPosition.y - camHeight / 2, 0), new Vector3(camPosition.x + camWidth / 2, camPosition.y + camHeight / 2, 0), Color.blue);
    }
    #endregion
}
