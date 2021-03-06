﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyShipPrefab;
    [SerializeField]
    private GameObject[] powerups;
    private GameManager _gameManager;

    private float enemySpawnTimer = 5.0f;

    private float powerupSpawnTimer = 5.0f;

	// Use this for initialization
	void Start ()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
	}
	

    public void StartSpawnRoutines()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(PowerUpSpawnRoutine());
    }
    //create coroutine to spawn enemy

    IEnumerator SpawnEnemyRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            Instantiate(enemyShipPrefab, new Vector3(Random.Range(-7.97f, 7.97f), 6.07f, 0), Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnTimer);
        }
    }

    IEnumerator PowerUpSpawnRoutine()
    {
        while (_gameManager.gameOver == false)
        {
            int randomPowerup = Random.Range(0, 3);

            Instantiate(powerups[randomPowerup], new Vector3(Random.Range(-7.97f, 7.97f), 6.07f, 0), Quaternion.identity);
            yield return new WaitForSeconds(powerupSpawnTimer);
        }
    }
}
