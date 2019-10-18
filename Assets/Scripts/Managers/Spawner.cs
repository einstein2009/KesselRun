using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    //This is only public until we tie in the difficulty
    public float spawnRate = 0.54f;
    //public float difficulty;
    public GameObject[] enemies;
    public GameObject[] obstacles;
    public GameObject[] powerups;

    public Transform MiddleSpawn;
    public Transform RightSpawn;
    public Transform LeftSpawn;
    public Transform TopRightSpawn;
    public Transform TopLeftSpawn;
    private Transform nextTransform;

    private GameObject nextSpawn;
    private GameObject enemySpawner;
    private int numOfEnemies;
    private int numOfObstacles;
    private int numOfPowerups;
    private float spawnCountdown;

    private GameObject player;


    void Start()
    {
        spawnCountdown = spawnRate;
        enemySpawner = this.gameObject;
        numOfEnemies = enemies.Length + 1;
        numOfObstacles = obstacles.Length + 1;
        numOfPowerups = powerups.Length;
        Random.InitState((int)System.DateTime.Now.Ticks);
        player = GameObject.FindGameObjectWithTag("Player");

        InvokeRepeating("IncreaseSpawnRate", 5f, 30f);
    }

    void IncreaseSpawnRate()
    {
        if (spawnRate > 0.25f)
            spawnRate -= 0.06f; //* player.GetComponent<PlayerMovement>().speedIncreaseCount;
        else
            spawnRate = 0.24f;
    }


    void Update()
    {
        //TODO: include difficulty to change the spawn rate.

        spawnCountdown -= Time.deltaTime;
        if (spawnCountdown < 0)
        {
            spawnCountdown = spawnRate;
            //Get the next random enemy or obstacle: both type and asset are random.
            nextTransform = GetTransform();
            //Debug.Log(nextTransform.position + " " + nextTransform.rotation);
            do
            {
                nextSpawn = GetNextSpawn(enemies, obstacles, powerups);
            } while (nextSpawn == null);
            GameObject newSpawn = Instantiate(nextSpawn);
            //newSpawn.transform.SetParent(nextTransform);
            newSpawn.transform.position = nextTransform.position;
            newSpawn.transform.Translate(0f, 0f, player.transform.position.z + 80);
            newSpawn.transform.rotation = nextTransform.rotation;

        }
    }

    private GameObject GetNextSpawn(GameObject[] enemies, GameObject[] obstacles, GameObject[] powerups) {

        //Enemy = 0
        //Obstacle = 1
        //Powerup = 2

        float enemyOrObstacleOrPowerup = Random.Range(0f, 1f);

        if (enemyOrObstacleOrPowerup < 0.60)
        {
            enemyOrObstacleOrPowerup = 0;
        } else if (enemyOrObstacleOrPowerup > 0.60 && enemyOrObstacleOrPowerup < 0.85)
        {
            enemyOrObstacleOrPowerup = 1;
        }
        else
        {
            enemyOrObstacleOrPowerup = 2;
        }

        if (enemyOrObstacleOrPowerup == 0 && enemies.Length != 0)
        {
            int randEnemy = Random.Range(0, numOfEnemies - 1);
            nextSpawn = enemies[randEnemy];
        }
        else if ((enemyOrObstacleOrPowerup == 1 && obstacles.Length != 0))
        {
            int randObstacle = Random.Range(0, numOfObstacles - 1);
            nextSpawn = obstacles[randObstacle];
        }
        else if ((enemyOrObstacleOrPowerup == 2 && powerups.Length != 0))
        {
            int randPowerup = Random.Range(0, numOfPowerups - 1);
            nextSpawn = powerups[randPowerup];
        } else
        {
            nextSpawn = null;
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
            default: return MiddleSpawn;
        }
    }
}