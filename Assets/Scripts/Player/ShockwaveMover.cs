using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveMover : MonoBehaviour
{
    public float speed = 30f;

    private GameObject player;
    PlayerMovement playerMovement;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }


    void Update()
    {
        speed = playerMovement.speed;
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);
    }
}
