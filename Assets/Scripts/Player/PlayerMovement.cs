using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 30.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public float transformLerpSpeed = 10.0f;
    public Transform topLeftTransform;
    public Transform leftTransform;
    public Transform middleTransform;
    public Transform rightTransform;
    public Transform topRightTransform;
    public Transform topTransform;
    public AudioSource movementSound;
    public int speedIncreaseCount = 0;
    public GameObject pipesParent;

    GameObject camera;
    SkyboxChange skyboxChange;
    private Vector3 moveDirection = Vector3.zero;
    private bool movingTop = false;
    private bool falling = false;
    private bool warping = false;
    private bool skyboxChanged = false;
    private float horizontalAxisValue;
    private float warpElapsed;
    private int currentLane = 0; // Middle
    private int targetLane = 0;
    private Vector3 targetVector3 = new Vector3();
    private Quaternion targetQuaternion = new Quaternion();
    private int lane = 0;

    void Start()
    {
        InvokeRepeating("IncreaseSpeed", 5f, 30f);
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        skyboxChange = camera.GetComponent<SkyboxChange>();
    }

    void IncreaseSpeed()
    {
        warping = true;
        speedIncreaseCount++;
        if (speed < 70)
        {
            speed += 10f;
        }
    }

    void Update()
    {
        // AUTOMATIC MOVING BEHAVIOR
        if (!falling)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World); //Keep Moving Forward
        } else
        {
            transform.Translate(Vector3.up * Time.deltaTime * 12f + Vector3.forward * Time.deltaTime * speed, Space.World);
            transform.Rotate(Vector3.right + Vector3.forward, 35 * Time.deltaTime, Space.Self);
            Camera.main.fieldOfView += 40 * Time.deltaTime;
        }

        if (warping)
        {
            TimeWarp();
        }

        // INPUT BEHAVIOR

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontalAxisValue = Input.GetAxis("Horizontal");
            //Debug.Log("firstkeypress");
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow) ||
          Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            horizontalAxisValue = Input.GetAxis("Horizontal");
            //Debug.Log("keypressed");
        }
        else
        {
            horizontalAxisValue = 0f;
            //Debug.Log("keyup");
        }

        // NEW LANE SWAP BEHAVIOR

        if (!movingTop)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                lane--;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                lane++;
            }
        }

        switch (lane)
        {
            case -2:
                targetVector3 = new Vector3(topLeftTransform.position.x, topLeftTransform.position.y, transform.position.z);
                targetQuaternion = topLeftTransform.rotation;
                PlayMovementSound();
                break;
            case -1:
                targetVector3 = new Vector3(leftTransform.position.x, leftTransform.position.y, transform.position.z);
                targetQuaternion = leftTransform.rotation;
                PlayMovementSound();
                break;
            case 0:
                targetVector3 = new Vector3(middleTransform.position.x, middleTransform.position.y, transform.position.z);
                targetQuaternion = middleTransform.rotation;
                PlayMovementSound();
                break;
            case 1:
                targetVector3 = new Vector3(rightTransform.position.x, rightTransform.position.y, transform.position.z);
                targetQuaternion = rightTransform.rotation;
                PlayMovementSound();
                break;
            case 2:
                targetVector3 = new Vector3(topRightTransform.position.x, topRightTransform.position.y, transform.position.z);
                targetQuaternion = topRightTransform.rotation;
                PlayMovementSound();
                break;
            default:
                targetVector3 = new Vector3(topTransform.position.x, topRightTransform.position.y, transform.position.z);
                targetQuaternion = topRightTransform.rotation;
                PlayMovementSound();
                break;
        }

        if (lane < -2 || lane > 2)
        {
            if (!falling)
                MoveTop();

            if (!IsInvoking("GameOver"))
                Invoke("GameOver", 3);
        }
        else
        {
            MoveToLane();
        }


    }

    void MoveToLane()
    {
        transform.position = Vector3.Lerp(transform.position, targetVector3, Time.deltaTime * transformLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * transformLerpSpeed);
    }

    // END GAME BEHAVIOR

    private void MoveTop()
    {
        movingTop = true;

        targetVector3 = new Vector3(topTransform.position.x, topTransform.position.y, transform.position.z);
        targetQuaternion = topTransform.rotation;

        if (Vector3.Distance(transform.position, targetVector3) < 0.09)
        {
            //Debug.Log("setting falling true");
            falling = true;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetVector3, Time.deltaTime * transformLerpSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQuaternion, Time.deltaTime * transformLerpSpeed);

    }

    private void GameOver()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().currentHealth = 0;
    }

    /*private bool isGrounded()
    {
        RaycastHit hit;
        float distance = 1f;
        //Vector3 dir = new Vector3(0, -1);
        Vector3 dir = -transform.up;

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            return true;
        }
        else
        {
            return false;
        }
    }*/

    // EXTRA BEHAVIOR

    /*void OnTriggerExit(Collider other)
    {
        //Debug.Log("Exiting " + other.gameObject.name);
        if (other.gameObject.CompareTag("Pipe"))
        {
            EndlessPipes localScript = other.gameObject.GetComponent<EndlessPipes>();
            if (localScript != null) ;
                localScript.MoveToFront();
        }

    }*/

    public void PlayMovementSound()
    {
        movementSound.Play();
    }

    void TimeWarp()
    {
        gameObject.GetComponent<PlayerHealth>().isImmune = true;
        warpElapsed += Time.deltaTime;

        if (warpElapsed < 1f) // first second
        {
            Camera.main.fieldOfView += 150 * Time.deltaTime; // camera zoom out
        }
        else // second second
        {
            if (Camera.main.fieldOfView > 60)
            {
                Camera.main.fieldOfView -= 150 * Time.deltaTime; // zoom back in
            }
            else
            {
                Camera.main.fieldOfView = 60; // normal cam view 60
            }
            if(skyboxChanged == false)
            {
                skyboxChange.ChangeMySkybox();
                skyboxChanged = true;
            }
        }
        if (warpElapsed > 2f)
        {
            warpElapsed = 0;
            warping = false;
            skyboxChanged = false;
            gameObject.GetComponent<PlayerHealth>().isImmune = false;
        }

    }
}
