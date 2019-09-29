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
    public GameObject[] barrierObstacles;

    public Transform MiddleSpawn;
    public Transform RightSpawn;
    public Transform LeftSpawn;
    public Transform TopRightSpawn;
    public Transform TopLeftSpawn;
    public Transform LeftBarrierSpawn;
    public Transform MiddleBarrierSpawn;
    public Transform RightBarrierSpawn;

    private GameObject nextSpawn;
    private GameObject nextBarrierSpawn;
    private GameObject enemySpawner;
    private int numOfEnemies;
    private int numOfObstacles;
    private int numOfPowerups;
    private int numOfBarrierObstacles;
    private float spawnCountdown;
    private float barrierSpawnCountdown;

    void Start()
    {
        spawnCountdown = spawnRate;
        barrierSpawnCountdown = .1f;
        enemySpawner = this.gameObject;
        numOfEnemies = enemies.Length;
        numOfObstacles = obstacles.Length;
        numOfPowerups = powerups.Length;
        numOfBarrierObstacles = barrierObstacles.Length;
        Random.InitState(42);
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

        barrierSpawnCountdown -= Time.deltaTime;
        if (barrierSpawnCountdown < 0)
        {
            //Get the next random enemy or obstacle: both type and asset are random.
            do
            {
                nextBarrierSpawn = GetNextBarrierSpawn(barrierObstacles);
            } while (nextBarrierSpawn == null);
            Instantiate(nextBarrierSpawn);
            barrierSpawnCountdown = .1f;
        }
    }

    private GameObject GetNextBarrierSpawn(GameObject[] barrierObjects)
    {
        Transform tempBarrierTransform = GetBarrierTransform();
        Random.InitState(System.DateTime.Now.Millisecond);

        if (enemies.Length != 0)
        {
            int randBarrier = Random.Range(0, numOfBarrierObstacles - 1);
            nextBarrierSpawn = barrierObstacles[randBarrier];
            nextBarrierSpawn.transform.position = tempBarrierTransform.position;
            nextBarrierSpawn.transform.rotation = tempBarrierTransform.rotation;
        }
        else
            nextBarrierSpawn = null;

        return nextBarrierSpawn;
    }

    private Transform GetBarrierTransform()
    {
        //Get random Loc
        int randLoc = Random.Range(0, 3);

        //Switch case to return a loc
        switch (randLoc)
        {
            case 0: return LeftBarrierSpawn;
            case 1: return MiddleBarrierSpawn;
            case 2: return RightBarrierSpawn;
            default: return transform;
        }
    }

    private GameObject GetNextSpawn(GameObject[] enemies, GameObject[] obstacles, GameObject[] powerups){

        Transform tempTransform = GetTransform();

        //Enemy = 0
        //Obstacle = 1
        //Powerup = 2

        int enemyOrObstacleOrPowerup = Random.Range(0, 3);

        if (enemyOrObstacleOrPowerup == 0 && enemies.Length != 0)
        {
            int randEnemy = Random.Range(0, numOfEnemies - 1);
            nextSpawn = enemies[randEnemy];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }
        else if ((enemyOrObstacleOrPowerup == 1 && obstacles.Length != 0))
        {
            int randObstacle = Random.Range(0, numOfObstacles - 1);
            nextSpawn = obstacles[randObstacle];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }
        else if ((enemyOrObstacleOrPowerup == 2 && powerups.Length != 0))
        {
            int randPowerup = Random.Range(0, numOfPowerups - 1);
            nextSpawn = powerups[randPowerup];
            nextSpawn.transform.position = tempTransform.position;
            nextSpawn.transform.rotation = tempTransform.rotation;
        }
        //multiple spawning issue fixed, it was only an issue when we didn't have obstacles populated.
        else
            nextSpawn = null;

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