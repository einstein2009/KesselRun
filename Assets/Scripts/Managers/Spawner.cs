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

        GameObject tempObject = GetLocAndRot();
        DestroyByTime tempDestr = tempObject.AddComponent<DestroyByTime>() as DestroyByTime;
        tempDestr.lifetime = 3;

        //Enemy = 0
        //Obstacle = 1
        int enemyOrObstacleOrPowerup = Random.Range(0, 3);

        if (enemyOrObstacleOrPowerup == 0 && enemies.Length != 0)
        {
            int randEnemy = Random.Range(0, numOfEnemies - 1);
            nextSpawn = enemies[randEnemy];
            nextSpawn.transform.localPosition = tempObject.transform.localPosition;
            nextSpawn.transform.localRotation = tempObject.transform.localRotation;
            nextSpawn.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if ((enemyOrObstacleOrPowerup == 1 && obstacles.Length != 0))
        {
            int randObstacle = Random.Range(0, numOfObstacles - 1);
            nextSpawn.transform.localPosition = tempObject.transform.localPosition;
            nextSpawn.transform.localRotation = tempObject.transform.localRotation;
            nextSpawn.transform.localScale = new Vector3(1,1,1);
            nextSpawn = obstacles[randObstacle];
        }
        else if ((enemyOrObstacleOrPowerup == 2 && powerups.Length != 0))
        {
            int randPowerup = Random.Range(0, numOfPowerups - 1);
            nextSpawn.transform.localPosition = tempObject.transform.localPosition;
            nextSpawn.transform.localRotation = tempObject.transform.localRotation;
            nextSpawn.transform.localScale = new Vector3(1, 1, 1);
            nextSpawn = powerups[randPowerup];
        }

        return nextSpawn;
    }

    private GameObject GetLocAndRot() {
        //Get random Lane
        int randLane = Random.Range(0, 5);

        //Switch case to return a lane
        switch (randLane)
        {
            case 0: return GetMiddleSpawn();
            case 1: return GetBottomRightSpawn();
            case 2: return GetBottomLeftSpawn();
            case 3: return GetTopRightSpawn();
            case 4: return GetTopLeftSpawn();
            default: return enemySpawner;
        }
    }

    private GameObject GetMiddleSpawn()
    {
        GameObject middleSpawn = new GameObject();
        Vector3 middleSpawnLocation = new Vector3(enemySpawner.transform.position.x, enemySpawner.transform.localPosition.y - 4.6f, enemySpawner.transform.position.z);
        Quaternion middleSpawnRotation = Quaternion.Euler(0, 180, 0);

        middleSpawn.transform.localPosition = middleSpawnLocation;
        middleSpawn.transform.localRotation = middleSpawnRotation;
        return middleSpawn;
    }

    private GameObject GetBottomRightSpawn()
    {
        GameObject bottomRightSpawn = new GameObject();
        Vector3 rightSpawnLocation = new Vector3(enemySpawner.transform.localPosition.x + 2.4f, enemySpawner.transform.localPosition.y - 3f, enemySpawner.transform.position.z);
        Quaternion rightSpawnRotation = Quaternion.Euler(0, 180, -60);

        bottomRightSpawn.transform.localPosition = rightSpawnLocation;
        bottomRightSpawn.transform.localRotation = rightSpawnRotation;
        return bottomRightSpawn;
    }


    private GameObject GetBottomLeftSpawn()
    {
        GameObject bottomLeftSpawn = new GameObject();
        Vector3 leftSpawnLocation = new Vector3(enemySpawner.transform.localPosition.x - 2.4f, enemySpawner.transform.localPosition.y - 3f, enemySpawner.transform.position.z);
        Quaternion leftSpawnRotation = Quaternion.Euler(0,180,60);

        bottomLeftSpawn.transform.localPosition = leftSpawnLocation;
        bottomLeftSpawn.transform.localRotation = leftSpawnRotation;
        return bottomLeftSpawn;
    }

    private GameObject GetTopRightSpawn()
    {
        GameObject topRightSpawn = new GameObject();
        Vector3 topRightSpawnLocation = new Vector3(enemySpawner.transform.localPosition.x + 2.4f, enemySpawner.transform.localPosition.y, enemySpawner.transform.position.z);
        Quaternion topRightSpawnRotation = Quaternion.Euler(0, 180, -120);

        topRightSpawn.transform.localPosition = topRightSpawnLocation;
        topRightSpawn.transform.localRotation = topRightSpawnRotation;
        return topRightSpawn;
    }

    private GameObject GetTopLeftSpawn()
    {
        GameObject topLeftSpawn = new GameObject();
        Vector3 topLeftSpawnLocation = new Vector3(enemySpawner.transform.localPosition.x - 2.4f, enemySpawner.transform.localPosition.y, enemySpawner.transform.position.z);
        Quaternion topLeftSpawnRotation = Quaternion.Euler(0, 180, 120);

        topLeftSpawn.transform.localPosition = topLeftSpawnLocation;
        topLeftSpawn.transform.localRotation = topLeftSpawnRotation;
        return topLeftSpawn;
    }
}