using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessPipes : MonoBehaviour
{
    private GameObject[] pipes;

    void Awake()
    {
        pipes = GameObject.FindGameObjectsWithTag("Pipe");
    }

    public void MoveToFront()
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
    }

    



}
