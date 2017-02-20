﻿using System.Collections;
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
    }
	
	public void SpawnEnemies(NetworkManager.EnemiesJSON enemiesJSON) //TODO networking
    {
        //TODO
        foreach (NetworkManager.UserJSON enemyJSON in enemiesJSON.enemies)
        {
            Debug.Log(enemyJSON.health);
            if (enemyJSON.health <= 0) //修复Client1打死的怪，Client2上线时还存在的bug
            {
                continue;
            }
            Vector3 position = new Vector3(enemyJSON.position[0], enemyJSON.position[1], enemyJSON.position[2]);
            Quaternion rotation = Quaternion.Euler(enemyJSON.rotation[0], enemyJSON.rotation[1], enemyJSON.rotation[2]);
            GameObject newEnemy = Instantiate(enemy, position, rotation) as GameObject;
            newEnemy.name = enemyJSON.name;
            PlayerController pc = newEnemy.GetComponent<PlayerController>();
            pc.isLocalPlayer = false;
            Health h = newEnemy.GetComponent<Health>();
            h.currentHealth = enemyJSON.health;
            h.OnChangeHealth();
            h.destroyOnDeath = true;
            h.isEnemy = true;
        }
	}
}
