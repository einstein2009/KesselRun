using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPipes : MonoBehaviour
{
    private GameObject[] pipes;
    private GameObject player;
    public float zTranslation = 100f;

    void Awake()
    {
        pipes = GameObject.FindGameObjectsWithTag("Pipe");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /*public void MoveToFront()
    {
        Vector3 furthestPosition = transform.position;
        foreach(GameObject pipe in pipes)
        {
            if(pipe.transform.position.z > furthestPosition.z)
            {
                furthestPosition = pipe.transform.position;
            }
        }

        furthestPosition.z += 10;
        this.transform.SetPositionAndRotation(furthestPosition, transform.rotation);
    }*/

    void Update()
    {
        if(transform.position.z - player.transform.position.z < -30)
        {
            MoveToFront();
        }
    }

    public void MoveToFront()
    {
        transform.Translate(Vector3.forward * zTranslation, Space.World);
    }





}
