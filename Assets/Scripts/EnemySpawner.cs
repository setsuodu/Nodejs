using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject spawnPoint;
    public int numberOfEnemies;
    [HideInInspector]
    public List<SpawnPoint> enemySpawnPoints;

    void Start()
    {
        // set the random spawn points over here
        for (int i = 0; i < numberOfEnemies; i++)
        {
            var spawnPosition = new Vector3(Random.Range(-8f, 8f), 0f, Random.Range(-8f, 8f));
            var spawnRotation = Quaternion.Euler(0, Random.Range(0, 180f), 0f);
            SpawnPoint enemySpawnPoint = (Instantiate(spawnPoint,
                                                      spawnPosition,
                                                      spawnRotation) as GameObject).GetComponent<SpawnPoint>();
            enemySpawnPoints.Add(enemySpawnPoint);
        }
        SpawnEnemies();
    }
	
	public void SpawnEnemies() //TODO networking
    {
        //TODO
        int i = 0;
        foreach (SpawnPoint sp in enemySpawnPoints)
        {
            Vector3 position = sp.transform.position;
            Quaternion rotation = sp.transform.rotation;
            GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
            enemy.name = i + "";
            PlayerController pc = newEnemy.GetComponent<PlayerController>();
            pc.isLocalPlayer = false;
            Health h = newEnemy.GetComponent<Health>();
            //h.OnChangeHealth();
            h.isEnemy = true;
            i++;
        }
	}
}
