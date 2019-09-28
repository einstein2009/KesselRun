using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //This is only public until we tie in the difficulty
    public float spawnRate;
    //public float difficulty;
    public GameObject[] enemies;
    public GameObject[] obstacles;
    public GameObject[] powerups;

    public Transform MiddleSpawn;
    public Transform RightSpawn;
    public Transform LeftSpawn;
    public Transform TopRightSpawn;
    public Transform TopLeftSpawn;

    private GameObject nextSpawn;
    private GameObject enemySpawner;
    private int numOfEnemies;
    private int numOfObstacles;
    private int numOfPowerups;
    private float spawnCountdown;

    void Start()
    {
        spawnCountdown = spawnRate;
        enemySpawner = this.gameObject;
        numOfEnemies = enemies.Length;
        numOfObstacles = obstacles.Length;
        numOfPowerups = powerups.Length;
    }


    void Update()
    {
        //TODO: include difficulty to change the spawn rate.

        
        spawnCountdown -= Time.deltaTime;
        if (spawnCountdown < 0)
        {
            //Get the next random enemy or obstacle: both type and asset are random.
            do
            {
                nextSpawn = GetNextSpawn(enemies, obstacles, powerups);
            } while (nextSpawn == null);
            Instantiate(nextSpawn);
            spawnCountdown = spawnRate;
        }
    }

    private GameObject GetNextSpawn(GameObject[] enemies, GameObject[] obstacles, GameObject[] powerups){

        Transform tempTransform = GetTransform();
        Random.InitState(System.DateTime.Now.Millisecond);

        //Enemy = 0
        //Obstacle = 1
        //Powerup = 2
        int enemyOrObstacleOrPowerup = Random.Range(0, 3);

        if (enemyOrObstacleOrPowerup == 0 && enemies.Length != 0)
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randEnemy = Random.Range(0, numOfEnemies - 1);
            nextSpawn = enemies[randEnemy];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }
        else if ((enemyOrObstacleOrPowerup == 1 && obstacles.Length != 0))
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randObstacle = Random.Range(0, numOfObstacles - 1);
            nextSpawn = obstacles[randObstacle];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }
        else if ((enemyOrObstacleOrPowerup == 2 && powerups.Length != 0))
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            int randPowerup = Random.Range(0, numOfPowerups - 1);
            nextSpawn = powerups[randPowerup];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }

        return nextSpawn;
    }

    private Transform GetTransform() {
        //Get random Lane
        int randLane = Random.Range(0, 5);

        //Switch case to return a lane
        switch (randLane)
        {
            case 0: return MiddleSpawn;
            case 1: return RightSpawn;
            case 2: return LeftSpawn;
            case 3: return TopRightSpawn;
            case 4: return TopLeftSpawn;
            default: return transform;
        }
    }


}